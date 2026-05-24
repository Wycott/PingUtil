using FluentAssertions;
using Moq;
using Pinger.Domain;
using Pinger.Interfaces;

namespace Pinger.Test;

public class PingEngineTest
{
    [Fact]
    public void Constructor_CreatesInstance()
    {
        var engine = CreateEngine();

        engine.Should().NotBeNull();
    }

    [Fact]
    public void Start_CallsDisplaySettings()
    {
        var mockPingDisplay = new Mock<IPingDisplay>();
        var engine = CreateEngine(pingDisplay: mockPingDisplay, pingCount: 1);

        engine.Start();

        mockPingDisplay.Verify(x => x.DisplaySettings(It.IsAny<string>(), It.IsAny<int>(),
            It.IsAny<byte[]>(), It.IsAny<ConsoleColor>(), It.IsAny<long>()), Times.Once);
    }

    [Fact]
    public void Start_CallsRecordPing()
    {
        var mockRollingStatistics = new Mock<IRollingStatistics>();
        var pingCount = 0L;

        mockRollingStatistics.Setup(x => x.TotalPings).Returns(() => pingCount);
        mockRollingStatistics.Setup(x => x.RecordPing(It.IsAny<IPingStats>()))
            .Callback(() => pingCount++)
            .Returns(100m);
        mockRollingStatistics.Setup(x => x.StopAfterThisManyPings).Returns(2);

        var engine = CreateEngine(rollingStatistics: mockRollingStatistics);

        engine.Start();

        mockRollingStatistics.Verify(x => x.RecordPing(It.IsAny<IPingStats>()), Times.Exactly(2));
    }

    [Fact]
    public void Start_CallsNotifyPingResult()
    {
        var mockConsoleHandler = new Mock<IConsoleHandler>();
        var engine = CreateEngine(consoleHandler: mockConsoleHandler, pingCount: 1);

        engine.Start();

        mockConsoleHandler.Verify(x => x.NotifyPingResult(It.IsAny<IPingStats>()), Times.Once);
    }

    [Fact]
    public void Start_CallsSetDisplayColour()
    {
        var mockPingDisplay = new Mock<IPingDisplay>();
        var engine = CreateEngine(pingDisplay: mockPingDisplay, pingCount: 1);

        engine.Start();

        mockPingDisplay.Verify(x => x.SetDisplayColour(It.IsAny<IPingStats>(), It.IsAny<decimal>(), It.IsAny<ConsoleColor>()), Times.Once);
    }

    [Fact]
    public void Start_CallsDisplayStatistics()
    {
        var mockPingDisplay = new Mock<IPingDisplay>();
        var engine = CreateEngine(pingDisplay: mockPingDisplay, pingCount: 3);

        engine.Start();

        mockPingDisplay.Verify(x => x.DisplayStatistics(It.IsAny<decimal>(), It.IsAny<IPingStats>(),
            It.IsAny<string>(), It.IsAny<ConsoleColor>(), It.IsAny<IRollingStatistics>()), Times.Exactly(3));
    }

    [Fact]
    public void Start_CallsFormatElapsedTime()
    {
        var mockPingTools = new Mock<IPingTools>();

        mockPingTools.Setup(x => x.CalculateWorkDayPings(It.IsAny<int>(), It.IsAny<int>())).Returns(1);
        mockPingTools.Setup(x => x.FormatElapsedTime(It.IsAny<TimeSpan>())).Returns("00:00:05");

        var engine = CreateEngine(pingTools: mockPingTools, pingCount: 1);

        engine.Start();

        mockPingTools.Verify(x => x.FormatElapsedTime(It.IsAny<TimeSpan>()), Times.AtLeastOnce);
    }

    private static IPingEngine CreateEngine(
        Mock<IPingTools>? pingTools = null,
        Mock<IPingDisplay>? pingDisplay = null,
        Mock<IConsoleHandler>? consoleHandler = null,
        Mock<IPingConfig>? pingConfig = null,
        Mock<IRollingStatistics>? rollingStatistics = null,
        int pingCount = 2)
    {
        pingTools ??= new Mock<IPingTools>();
        pingTools.Setup(x => x.CalculateWorkDayPings(It.IsAny<int>(), It.IsAny<int>())).Returns(pingCount);
        pingTools.Setup(x => x.FormatElapsedTime(It.IsAny<TimeSpan>())).Returns("00:00:00");

        pingDisplay ??= new Mock<IPingDisplay>();
        consoleHandler ??= new Mock<IConsoleHandler>();

        pingConfig ??= new Mock<IPingConfig>();
        pingConfig.Setup(x => x.Data).Returns("test");
        pingConfig.Setup(x => x.PingerIsActive).Returns(false);

        if (rollingStatistics == null)
        {
            rollingStatistics = new Mock<IRollingStatistics>();

            var count = 0L;

            rollingStatistics.Setup(x => x.TotalPings).Returns(() => count);
            rollingStatistics.Setup(x => x.RecordPing(It.IsAny<IPingStats>()))
                .Callback(() => count++)
                .Returns(100m);
            rollingStatistics.Setup(x => x.StopAfterThisManyPings).Returns(pingCount);
        }

        return new PingEngine(pingTools.Object, pingDisplay.Object,
            consoleHandler.Object, pingConfig.Object, rollingStatistics.Object);
    }
}
