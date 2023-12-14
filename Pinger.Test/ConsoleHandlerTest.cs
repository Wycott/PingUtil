using FluentAssertions;
using Moq;
using Pinger.Interfaces;

namespace Pinger.Test;

public class ConsoleHandlerTest
{
    [Fact]
    public void WriteToConsole()
    {
        // Arrange/Act
        IConsoleHandler consoleHandler = new ConsoleHandler(GetPingConfigMock());
        consoleHandler.WriteToConsole("Message");

        // Assert
        consoleHandler.Should().NotBeNull();
    }

    [Fact]
    public void ForegroundColour()
    {
        // Arrange
        IConsoleHandler consoleHandler = new ConsoleHandler(GetPingConfigMock())
        {
            ForegroundColour = ConsoleColor.Red
        };

        // Act
        _ = consoleHandler.ForegroundColour;

        // Assert
        consoleHandler.Should().NotBeNull();
    }

    [Fact]
    public void AudioCue_WhenSuccess()
    {
        // Arrange
        IConsoleHandler consoleHandler = new ConsoleHandler(GetPingConfigMock());

        // Act
        consoleHandler.AudioCue(new PingStats(), 5);

        // Assert
        consoleHandler.Should().NotBeNull();
    }

    [Fact]
    public void AudioCue_WhenFailed()
    {
        // Arrange
        IConsoleHandler consoleHandler = new ConsoleHandler(GetPingConfigMock());

        // Act
        consoleHandler.AudioCue(new PingStats() { Success = true }, 0);

        // Assert
        consoleHandler.Should().NotBeNull();
    }

    private static IPingConfig GetPingConfigMock()
    {
        return new Mock<IPingConfig>().Object;
    }
}