using FluentAssertions;
using Pinger.Interfaces;

namespace Pinger.Test;

public class ConsoleHandlerTest
{
    [Fact]
    public void WriteToConsole()
    {
        // Arrange/Act
        IConsoleHandler consoleHandler = new ConsoleHandler();
        consoleHandler.WriteToConsole("Message");

        // Assert
        consoleHandler.Should().NotBeNull();
    }

    [Fact]
    public void ForegroundColour()
    {
        // Arrange
        IConsoleHandler consoleHandler = new ConsoleHandler();

        // Act
        consoleHandler.ForegroundColour = ConsoleColor.Red;
        _ = consoleHandler.ForegroundColour;

        // Assert
        consoleHandler.Should().NotBeNull();
    }

    [Fact]
    public void AudioCue_WhenSuccess()
    {
        // Arrange
        IConsoleHandler consoleHandler = new ConsoleHandler();

        // Act
        consoleHandler.AudioCue(new PingStats(), 5);

        // Assert
        consoleHandler.Should().NotBeNull();
    }

    [Fact]
    public void AudioCue_WhenFailed()
    {
        // Arrange
        IConsoleHandler consoleHandler = new ConsoleHandler();

        // Act
        consoleHandler.AudioCue(new PingStats() { Success = true }, 0);

        // Assert
        consoleHandler.Should().NotBeNull();
    }
}