using FluentAssertions;
using Moq;
using Pinger.Domain;
using Pinger.Interfaces;

namespace Pinger.Test;

// Preservation tests for the "ping exception message leak" bugfix.
// Property 2: Preservation - Non-Throwing And Inactive Behavior.
//
// These tests capture the baseline behavior that MUST remain unchanged by the fix.
//
// NOTE ON SCOPE: PingHost calls `new Ping().Send(...)` directly (Ping is not
// injectable), so the success and non-success-no-throw paths cannot be exercised
// deterministically in a unit test - they depend on the host machine's actual ICMP
// behavior, which differs between developer machines and CI runners (e.g. raw ICMP
// to loopback is not permitted for unprivileged processes on Linux). Those paths
// were removed to keep the suite deterministic; they assert the behavior of
// System.Net.NetworkInformation.Ping rather than of this codebase. The behaviors
// that ARE owned by this codebase and are environment-independent remain covered:
//   - PingerIsActive == false -> PingStats { Success = true } (short-circuit)
//   - Send throws             -> failed PingStats (Success == false) via the catch
//
// Validates: Requirements 3.1, 3.3
public class PingPreservationTest
{
    // Reliably fails to resolve so that Ping.Send(...) throws.
    private const string UnresolvableHost = "this.host.does.not.exist.invalid";

    // Property: for any inactive-pinger configuration, the flow records a successful
    // PingStats via the short-circuit, regardless of the configured remote server.
    [Theory]
    [InlineData("127.0.0.1")]
    [InlineData("192.0.2.1")]
    [InlineData("this.host.does.not.exist.invalid")]
    [InlineData("example.com")]
    public void Preserves_InactivePinger_RecordsSuccessTrue(string remoteServer)
    {
        var recordedStats = RunSinglePingAndCapture(pingerIsActive: false, remoteServer: remoteServer);

        recordedStats.Should().NotBeNull("the inactive short-circuit still records a ping result");
        recordedStats!.Success.Should().BeTrue("PingerIsActive == false short-circuits to a successful PingStats");
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