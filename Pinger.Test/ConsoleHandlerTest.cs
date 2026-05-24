using FluentAssertions;
using Moq;
using Pinger.Domain;
using Pinger.Interfaces;

namespace Pinger.Test;

public class ConsoleHandlerTest
{
    [Fact]
    public void WriteToConsole_DoesNotThrow()
    {
        IConsoleHandler consoleHandler = new ConsoleHandler(GetPingConfigMock());
        consoleHandler.WriteToConsole("Message");

        consoleHandler.Should().NotBeNull();
    }

    [Fact]
    public void ForegroundColour_CanBeSetWithoutThrowing()
    {
        IConsoleHandler consoleHandler = new ConsoleHandler(GetPingConfigMock());

        var act = () => consoleHandler.ForegroundColour = ConsoleColor.Red;

        act.Should().NotThrow();
    }

    [Fact]
    public void NotifyPingResult_WhenSuccess_DoesNotBeep()
    {
        var config = new Mock<IPingConfig>();
        var consoleHandler = new SilentConsoleHandler(config.Object);

        consoleHandler.NotifyPingResult(new PingStats { Success = true });

        consoleHandler.BeepCount.Should().Be(0);
    }

    [Fact]
    public void NotifyPingResult_WhenFailedBelowThreshold_DoesNotBeep()
    {
        var config = new Mock<IPingConfig>();

        config.Setup(x => x.AlertAfterThisManyFailedPings).Returns(5);

        var consoleHandler = new SilentConsoleHandler(config.Object);

        consoleHandler.NotifyPingResult(new PingStats { Success = false });
        consoleHandler.NotifyPingResult(new PingStats { Success = false });

        consoleHandler.BeepCount.Should().Be(0);
    }

    [Fact]
    public void NotifyPingResult_WhenFailedAtThreshold_Beeps()
    {
        var config = new Mock<IPingConfig>();

        config.Setup(x => x.AlertAfterThisManyFailedPings).Returns(3);

        var consoleHandler = new SilentConsoleHandler(config.Object);

        consoleHandler.NotifyPingResult(new PingStats { Success = false });
        consoleHandler.NotifyPingResult(new PingStats { Success = false });
        consoleHandler.NotifyPingResult(new PingStats { Success = false });

        consoleHandler.BeepCount.Should().Be(1);
    }

    [Fact]
    public void NotifyPingResult_ContinuedFailuresBeyondThreshold_KeepsBeeping()
    {
        var config = new Mock<IPingConfig>();

        config.Setup(x => x.AlertAfterThisManyFailedPings).Returns(2);

        var consoleHandler = new SilentConsoleHandler(config.Object);

        consoleHandler.NotifyPingResult(new PingStats { Success = false });
        consoleHandler.NotifyPingResult(new PingStats { Success = false }); // threshold hit
        consoleHandler.NotifyPingResult(new PingStats { Success = false }); // still above
        consoleHandler.NotifyPingResult(new PingStats { Success = false }); // still above

        consoleHandler.BeepCount.Should().Be(3);
    }

    [Fact]
    public void NotifyPingResult_WhenSuccess_ResetsCluster()
    {
        var config = new Mock<IPingConfig>();

        config.Setup(x => x.AlertAfterThisManyFailedPings).Returns(2);

        var consoleHandler = new SilentConsoleHandler(config.Object);

        // Fail once (below threshold)
        consoleHandler.NotifyPingResult(new PingStats { Success = false });

        // Success resets
        consoleHandler.NotifyPingResult(new PingStats { Success = true });

        // Fail once more - still below threshold since reset
        consoleHandler.NotifyPingResult(new PingStats { Success = false });

        consoleHandler.BeepCount.Should().Be(0);
    }

    [Fact]
    public void NotifyPingResult_AfterReset_CanReachThresholdAgain()
    {
        var config = new Mock<IPingConfig>();

        config.Setup(x => x.AlertAfterThisManyFailedPings).Returns(2);

        var consoleHandler = new SilentConsoleHandler(config.Object);

        // First cluster hits threshold
        consoleHandler.NotifyPingResult(new PingStats { Success = false });
        consoleHandler.NotifyPingResult(new PingStats { Success = false });
        consoleHandler.BeepCount.Should().Be(1);

        // Reset
        consoleHandler.NotifyPingResult(new PingStats { Success = true });

        // Second cluster hits threshold
        consoleHandler.NotifyPingResult(new PingStats { Success = false });
        consoleHandler.NotifyPingResult(new PingStats { Success = false });
        consoleHandler.BeepCount.Should().Be(2);
    }

    private static IPingConfig GetPingConfigMock() => new Mock<IPingConfig>().Object;

    private class SilentConsoleHandler(IPingConfig pingConfig) : ConsoleHandler(pingConfig)
    {
        public int BeepCount { get; private set; }
        protected override void Beep() => BeepCount++;
    }
}
