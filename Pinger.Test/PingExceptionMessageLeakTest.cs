using FluentAssertions;
using Moq;
using Pinger.Domain;
using Pinger.Interfaces;

namespace Pinger.Test;

// Bug condition exploration test for the "ping exception message leak" bugfix.
// Property 1: Bug Condition - Friendly Exception Message.
// This test encodes the EXPECTED (post-fix) behavior. On UNFIXED code it is
// expected to FAIL, which confirms that the internal exception details leak.
public class PingExceptionMessageLeakTest
{
    // A host that reliably fails to resolve so that Ping.Send(...) throws.
    private const string UnresolvableHost = "this.host.does.not.exist.invalid";

    // The .NET exception type name that currently leaks to the user.
    private const string LeakedExceptionTypeName = "PingException";

    // The raw internal exception message that currently leaks to the user.
    private const string LeakedInternalMessage = "An exception occurred during a Ping request.";

    [Fact]
    public void PingHost_WhenSendThrows_WritesFriendlyMessageWithoutInternalDetails()
    {
        var capturedMessages = new List<string>();

        var mockConsoleHandler = new Mock<IConsoleHandler>();
        mockConsoleHandler
            .Setup(x => x.WriteToConsole(It.IsAny<string>()))
            .Callback<string>(message => capturedMessages.Add(message));

        var mockPingTools = new Mock<IPingTools>();
        mockPingTools.Setup(x => x.CalculateWorkDayPings(It.IsAny<int>(), It.IsAny<int>())).Returns(1);
        mockPingTools.Setup(x => x.FormatElapsedTime(It.IsAny<TimeSpan>())).Returns("00:00:00");

        var mockPingConfig = new Mock<IPingConfig>();
        mockPingConfig.Setup(x => x.Data).Returns("test");
        mockPingConfig.Setup(x => x.PingerIsActive).Returns(true);
        mockPingConfig.Setup(x => x.RemoteServer).Returns(UnresolvableHost);
        mockPingConfig.Setup(x => x.Timeout).Returns(100);

        var recordedStats = new List<IPingStats>();
        var mockRollingStatistics = new Mock<IRollingStatistics>();
        var pingCount = 0L;
        mockRollingStatistics.Setup(x => x.TotalPings).Returns(() => pingCount);
        mockRollingStatistics.Setup(x => x.StopAfterThisManyPings).Returns(1);
        mockRollingStatistics
            .Setup(x => x.RecordPing(It.IsAny<IPingStats>()))
            .Callback<IPingStats>(stats =>
            {
                recordedStats.Add(stats);
                pingCount++;
            })
            .Returns(0m);

        var engine = new PingEngine(
            mockPingTools.Object,
            new Mock<IPingDisplay>().Object,
            mockConsoleHandler.Object,
            mockPingConfig.Object,
            mockRollingStatistics.Object);

        engine.Start();

        // The exception path must have been reached and written a message.
        capturedMessages.Should().NotBeEmpty("Ping.Send should throw for an unresolvable host and write an error message");

        var writtenMessage = capturedMessages[0];

        // Expected (post-fix) behavior: the message must not leak internal details.
        writtenMessage.Should().NotContain(LeakedExceptionTypeName,
            "the friendly message must not expose the .NET exception type name");
        writtenMessage.Should().NotContain(LeakedInternalMessage,
            "the friendly message must not expose the raw internal exception message");

        // Preserved behavior: an exception still yields a failed PingStats.
        recordedStats.Should().NotBeEmpty();
        recordedStats[0].Success.Should().BeFalse("an exception must still produce a failed ping result");
    }
}
