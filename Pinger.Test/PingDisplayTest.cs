using System.Text;
using FluentAssertions;
using Moq;
using Pinger.Domain;
using Pinger.Interfaces;

namespace Pinger.Test;

public class PingDisplayTest
{
    [Fact]
    public void DisplayStatistics()
    {
        // Arrange
        var mockConsole = GetMockConsoleHandler();
        var mockPingConfig = GetMockPingConfig();
        var mockRollingStatistics = GetMockRollingStatistics();

        IPingDisplay pingDisplay = new PingDisplay(mockConsole.Object, mockPingConfig.Object);

        // Act
        pingDisplay.DisplayStatistics(0, new PingStats(), string.Empty, ConsoleColor.DarkMagenta, mockRollingStatistics.Object);

        // Assert
        mockConsole.Verify(x => x.WriteToConsole(It.IsAny<string>()), Times.Once());
        pingDisplay.Should().NotBeNull();
    }

    [Fact]
    public void SetDisplayColour()
    {
        // Arrange
        var mockConsole = GetMockConsoleHandler();
        var mockPingConfig = GetMockPingConfig();

        IPingDisplay pingDisplay = new PingDisplay(mockConsole.Object, mockPingConfig.Object);

        // Act
        pingDisplay.SetDisplayColour(new PingStats() { PingTime = 1 }, 0);

        // Assert
        pingDisplay.Should().NotBeNull();
        mockConsole.Verify(x => x.WriteToConsole(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void DisplaySettings()
    {
        // Arrange
        var mockConsole = GetMockConsoleHandler();
        var mockPingConfig = GetMockPingConfig();

        IPingDisplay pingDisplay = new PingDisplay(mockConsole.Object, mockPingConfig.Object);

        // Act
        pingDisplay.DisplaySettings("", 0, Encoding.ASCII.GetBytes("Something"), 0, ConsoleColor.Cyan, 0);

        // Assert
        pingDisplay.Should().NotBeNull();
        mockConsole.Verify(x => x.WriteToConsole(It.IsAny<string>()), Times.Exactly(2));
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