using FluentAssertions;
using Moq;
using Pinger.Domain;
using Pinger.Interfaces;

namespace Pinger.Test;

// Preservation tests for the "ping exception message leak" bugfix.
// Property 2: Preservation - Non-Throwing And Inactive Behavior.
//
// These tests capture the baseline behavior that MUST remain unchanged by the fix.
// They are written observation-first: the asserted outcomes were recorded by
// running the current (unfixed) code, and they are EXPECTED TO PASS on unfixed code.
//
// Observed baseline (unfixed code, via direct System.Net.NetworkInformation.Ping):
//   - PingerIsActive == false          -> PingStats { Success = true }  (short-circuit)
//   - Send("127.0.0.1") succeeds        -> PingStats { Success = true,  PingTime = reply.RoundtripTime }
//   - Send("192.0.2.1") times out       -> PingStats { Success = false, PingTime = reply.RoundtripTime } (no throw)
//   - Send("<unresolvable>") throws      -> PingStats { Success = false } (failed result via catch)
//
// Because PingHost calls `new Ping().Send(...)` directly (Ping is not injectable),
// the success and non-success-no-throw paths are exercised against real, deterministic
// endpoints: the loopback address 127.0.0.1 (reliably succeeds) and 192.0.2.1
// (TEST-NET-1, reserved and non-routable per RFC 5737, so it reliably times out
// without throwing). The exception path uses an unresolvable host, and the inactive
// path is fully controlled via IPingConfig.PingerIsActive == false.
//
// Validates: Requirements 3.1, 3.2, 3.3
public class PingPreservationTest
{
    // Loopback: Send reliably returns IPStatus.Success without throwing.
    private const string SuccessHost = "127.0.0.1";

    // RFC 5737 TEST-NET-1: reserved, non-routable; Send reliably times out (non-success, no throw).
    private const string NonSuccessNoThrowHost = "192.0.2.1";

    // Reliably fails to resolve so that Ping.Send(...) throws.
    private const string UnresolvableHost = "this.host.does.not.exist.invalid";

    // Property: for any inactive-pinger configuration, the flow records a successful
    // PingStats via the short-circuit, regardless of the configured remote server.
    [Theory]
    [InlineData(SuccessHost)]
    [InlineData(NonSuccessNoThrowHost)]
    [InlineData(UnresolvableHost)]
    [InlineData("example.com")]
    public void Preserves_InactivePinger_RecordsSuccessTrue(string remoteServer)
    {
        var recordedStats = RunSinglePingAndCapture(pingerIsActive: false, remoteServer: remoteServer);

        recordedStats.Should().NotBeNull("the inactive short-circuit still records a ping result");
        recordedStats!.Success.Should().BeTrue("PingerIsActive == false short-circuits to a successful PingStats");
    }

    // Property: for a reachable host, an active pinger records a successful PingStats
    // carrying the reply's round-trip time (>= 0).
    [Fact]
    public void Preserves_SuccessfulPing_RecordsSuccessTrueWithPingTime()
    {
        var recordedStats = RunSinglePingAndCapture(pingerIsActive: true, remoteServer: SuccessHost);

        recordedStats.Should().NotBeNull();
        recordedStats!.Success.Should().BeTrue("a successful reply from the loopback address yields Success = true");
        recordedStats.PingTime.Should().BeGreaterThanOrEqualTo(0, "the recorded PingTime reflects the reply round-trip time");
    }

    // Property: for a non-success reply that does not throw (a timeout), an active
    // pinger records a failed PingStats carrying the reply's round-trip time (>= 0).
    [Fact]
    public void Preserves_NonSuccessNoThrow_RecordsSuccessFalseWithPingTime()
    {
        var recordedStats = RunSinglePingAndCapture(pingerIsActive: true, remoteServer: NonSuccessNoThrowHost);

        recordedStats.Should().NotBeNull();
        recordedStats!.Success.Should().BeFalse("a non-success reply (timeout) that does not throw yields Success = false");
        recordedStats.PingTime.Should().BeGreaterThanOrEqualTo(0, "the recorded PingTime reflects the reply round-trip time");
    }

    // Property: for any host that causes Send to throw, an active pinger records a
    // failed PingStats via the catch block, keeping statistics and alerting accurate.
    [Fact]
    public void Preserves_ExceptionPath_RecordsFailedPingStats()
    {
        var recordedStats = RunSinglePingAndCapture(pingerIsActive: true, remoteServer: UnresolvableHost);

        recordedStats.Should().NotBeNull("an exception still produces a recorded ping result");
        recordedStats!.Success.Should().BeFalse("an exception yields a failed PingStats (Success == false)");
    }

    // Drives PingEngine.Start() for exactly one ping and returns the IPingStats
    // captured through the mocked IRollingStatistics.RecordPing, which is the value
    // PingHost produced for the given configuration.
    private static IPingStats? RunSinglePingAndCapture(bool pingerIsActive, string remoteServer)
    {
        var mockPingTools = new Mock<IPingTools>();
        mockPingTools.Setup(x => x.CalculateWorkDayPings(It.IsAny<int>(), It.IsAny<int>())).Returns(1);
        mockPingTools.Setup(x => x.FormatElapsedTime(It.IsAny<TimeSpan>())).Returns("00:00:00");

        var mockPingConfig = new Mock<IPingConfig>();
        mockPingConfig.Setup(x => x.Data).Returns("test");
        mockPingConfig.Setup(x => x.PingerIsActive).Returns(pingerIsActive);
        mockPingConfig.Setup(x => x.RemoteServer).Returns(remoteServer);
        mockPingConfig.Setup(x => x.Timeout).Returns(1000);

        IPingStats? captured = null;
        var pingCount = 0L;

        var mockRollingStatistics = new Mock<IRollingStatistics>();
        mockRollingStatistics.Setup(x => x.TotalPings).Returns(() => pingCount);
        mockRollingStatistics.Setup(x => x.StopAfterThisManyPings).Returns(1);
        mockRollingStatistics
            .Setup(x => x.RecordPing(It.IsAny<IPingStats>()))
            .Callback<IPingStats>(stats =>
            {
                captured = stats;
                pingCount++;
            })
            .Returns(0m);

        var engine = new PingEngine(
            mockPingTools.Object,
            new Mock<IPingDisplay>().Object,
            new Mock<IConsoleHandler>().Object,
            mockPingConfig.Object,
            mockRollingStatistics.Object);

        engine.Start();

        return captured;
    }
}
