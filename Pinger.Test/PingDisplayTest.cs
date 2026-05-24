using FluentAssertions;
using Moq;
using Pinger.Domain;
using Pinger.Interfaces;
using System.Text;

namespace Pinger.Test;

public class PingDisplayTest
{
    [Fact]
    public void DisplayStatistics_WritesToConsole()
    {
        var mockConsole = GetMockConsoleHandler();
        var mockPingConfig = GetMockPingConfig();
        var mockRollingStatistics = GetMockRollingStatistics();

        IPingDisplay pingDisplay = new PingDisplay(mockConsole.Object, mockPingConfig.Object);

        pingDisplay.DisplayStatistics(95.5m, new PingStats { Success = true, PingTime = 12 }, "01:00:00", ConsoleColor.Gray, mockRollingStatistics.Object);

        mockConsole.Verify(x => x.WriteToConsole(It.IsAny<string>()), Times.Once());
    }

    [Fact]
    public void DisplayStatistics_ResetsColourToUsual()
    {
        var mockConsole = GetMockConsoleHandler();
        var mockPingConfig = GetMockPingConfig();
        var mockRollingStatistics = GetMockRollingStatistics();

        IPingDisplay pingDisplay = new PingDisplay(mockConsole.Object, mockPingConfig.Object);

        pingDisplay.DisplayStatistics(100m, new PingStats(), "00:00:00", ConsoleColor.Cyan, mockRollingStatistics.Object);

        mockConsole.VerifySet(x => x.ForegroundColour = ConsoleColor.Cyan, Times.Once);
    }

    [Fact]
    public void DisplayStatistics_IncludesCountdownInOutput()
    {
        var mockConsole = GetMockConsoleHandler();
        var mockPingConfig = GetMockPingConfig();
        mockPingConfig.Setup(x => x.SnoozeTime).Returns(5000);
        var mockRollingStatistics = new Mock<IRollingStatistics>();
        mockRollingStatistics.Setup(x => x.StopAfterThisManyPings).Returns(100);
        mockRollingStatistics.Setup(x => x.TotalPings).Returns(10);

        IPingDisplay pingDisplay = new PingDisplay(mockConsole.Object, mockPingConfig.Object);

        pingDisplay.DisplayStatistics(100m, new PingStats(), "00:00:00", ConsoleColor.Gray, mockRollingStatistics.Object);

        mockConsole.Verify(x => x.WriteToConsole(It.Is<string>(s => s.Contains("Remaining:90") && s.Contains("("))));
    }

    [Fact]
    public void SetDisplayColour_WhenFailed_SetsRedColour()
    {
        var mockConsole = GetMockConsoleHandler();
        var mockPingConfig = GetMockPingConfig();
        IPingDisplay pingDisplay = new PingDisplay(mockConsole.Object, mockPingConfig.Object);

        pingDisplay.SetDisplayColour(new PingStats { Success = false }, 10, ConsoleColor.Gray);

        mockConsole.VerifySet(x => x.ForegroundColour = ConsoleColor.Red, Times.Once);
    }

    [Fact]
    public void SetDisplayColour_WhenPingTimeExceedsAverage_SetsWhiteColour()
    {
        var mockConsole = GetMockConsoleHandler();
        var mockPingConfig = GetMockPingConfig();
        IPingDisplay pingDisplay = new PingDisplay(mockConsole.Object, mockPingConfig.Object);

        pingDisplay.SetDisplayColour(new PingStats { Success = true, PingTime = 20 }, 10, ConsoleColor.Gray);

        mockConsole.VerifySet(x => x.ForegroundColour = ConsoleColor.White, Times.Once);
    }

    [Fact]
    public void SetDisplayColour_WhenSuccessAndBelowAverage_SetsUsualColour()
    {
        var mockConsole = GetMockConsoleHandler();
        var mockPingConfig = GetMockPingConfig();
        IPingDisplay pingDisplay = new PingDisplay(mockConsole.Object, mockPingConfig.Object);

        pingDisplay.SetDisplayColour(new PingStats { Success = true, PingTime = 5 }, 10, ConsoleColor.DarkYellow);

        mockConsole.VerifySet(x => x.ForegroundColour = ConsoleColor.DarkYellow, Times.Once);
    }

    [Fact]
    public void SetDisplayColour_WhenFailedWithHighPingTime_SetsRedNotWhite()
    {
        var mockConsole = GetMockConsoleHandler();
        var mockPingConfig = GetMockPingConfig();
        IPingDisplay pingDisplay = new PingDisplay(mockConsole.Object, mockPingConfig.Object);

        pingDisplay.SetDisplayColour(new PingStats { Success = false, PingTime = 100 }, 10, ConsoleColor.Gray);

        mockConsole.VerifySet(x => x.ForegroundColour = ConsoleColor.Red, Times.Once);
        mockConsole.VerifySet(x => x.ForegroundColour = ConsoleColor.White, Times.Never);
    }

    [Fact]
    public void DisplaySettings_SetsYellowColour()
    {
        var mockConsole = GetMockConsoleHandler();
        var mockPingConfig = GetMockPingConfig();
        IPingDisplay pingDisplay = new PingDisplay(mockConsole.Object, mockPingConfig.Object);

        pingDisplay.DisplaySettings("8.8.8.8", 10000, Encoding.ASCII.GetBytes("test"), 5000, ConsoleColor.Gray, 100);

        mockConsole.VerifySet(x => x.ForegroundColour = ConsoleColor.Yellow, Times.Once);
    }

    [Fact]
    public void DisplaySettings_WritesHostAndCodeName()
    {
        var mockConsole = GetMockConsoleHandler();
        var mockPingConfig = GetMockPingConfig();
        mockPingConfig.Setup(x => x.CodeName).Returns("TestCode");
        IPingDisplay pingDisplay = new PingDisplay(mockConsole.Object, mockPingConfig.Object);

        pingDisplay.DisplaySettings("8.8.8.8", 10000, Encoding.ASCII.GetBytes("test"), 5000, ConsoleColor.Gray, 100);

        mockConsole.Verify(x => x.WriteToConsole(It.Is<string>(s => s.Contains("8.8.8.8"))), Times.Once);
        mockConsole.Verify(x => x.WriteToConsole(It.Is<string>(s => s.Contains("TestCode"))), Times.Once);
    }

    [Fact]
    public void DisplaySettings_ResetsColourToUsual()
    {
        var mockConsole = GetMockConsoleHandler();
        var mockPingConfig = GetMockPingConfig();
        IPingDisplay pingDisplay = new PingDisplay(mockConsole.Object, mockPingConfig.Object);

        pingDisplay.DisplaySettings("8.8.8.8", 10000, Encoding.ASCII.GetBytes("test"), 5000, ConsoleColor.Cyan, 100);

        mockConsole.VerifySet(x => x.ForegroundColour = ConsoleColor.Cyan, Times.Once);
    }

    private static Mock<IConsoleHandler> GetMockConsoleHandler()
    {
        return new Mock<IConsoleHandler>();
    }

    private static Mock<IPingConfig> GetMockPingConfig()
    {
        return new Mock<IPingConfig>();
    }

    private static Mock<IRollingStatistics> GetMockRollingStatistics()
    {
        return new Mock<IRollingStatistics>();
    }
}
