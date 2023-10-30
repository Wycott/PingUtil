using FluentAssertions;
using Moq;
using Pinger.Interfaces;
using System.Text;

namespace Pinger.Test;

public class PingDisplayTest
{
    [Fact]
    public void DisplayStatistics()
    {
        // Arrange
        var mockConsole = GetMockConsoleHandler();
        IPingDisplay pingDisplay = new PingDisplay(mockConsole.Object);

        // Act
        pingDisplay.DisplayStatistics(0, new PingStats(), 0, 0, 0, 0, 0, 0, "", 0, ConsoleColor.DarkMagenta);

        // Assert
        mockConsole.Verify(x => x.WriteToConsole(It.IsAny<string>()), Times.Once());
        pingDisplay.Should().NotBeNull();
    }

    [Fact]
    public void SetDisplayColour()
    {
        // Arrange
        var mockConsole = GetMockConsoleHandler();
        IPingDisplay pingDisplay = new PingDisplay(mockConsole.Object);

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
        IPingDisplay pingDisplay = new PingDisplay(mockConsole.Object);

        // Act
        pingDisplay.DisplaySettings("", 0, Encoding.ASCII.GetBytes("Something"), 0, ConsoleColor.Cyan, 0);

        // Assert
        pingDisplay.Should().NotBeNull();
        mockConsole.Verify(x => x.WriteToConsole(It.IsAny<string>()), Times.Once());
    }

    private Mock<IConsoleHandler> GetMockConsoleHandler()
    {
        return new Mock<IConsoleHandler>();
    }
}