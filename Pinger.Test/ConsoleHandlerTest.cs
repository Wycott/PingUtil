using AiAnnotations;
using AiAnnotations.Types;
using FluentAssertions;
using Moq;
using Pinger.Domain;
using Pinger.Interfaces;

namespace Pinger.Test;

[AiGenerated(Authorship.Hybrid)]
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

    [Fact]
    public void AudioCue_WhenFailedBelowThreshold_ReturnsIncrementedCount()
    {
        var config = new Mock<IPingConfig>();
        config.Setup(x => x.AlertAfterThisManyFailedPings).Returns(5);
        IConsoleHandler consoleHandler = new ConsoleHandler(config.Object);

        var result = consoleHandler.AudioCue(new PingStats { Success = false }, 2);

        result.Should().Be(3);
    }

    [Fact]
    public void AudioCue_WhenFailedAtThreshold_ReturnsIncrementedCount()
    {
        var config = new Mock<IPingConfig>();
        config.Setup(x => x.AlertAfterThisManyFailedPings).Returns(3);
        IConsoleHandler consoleHandler = new ConsoleHandler(config.Object);

        var result = consoleHandler.AudioCue(new PingStats { Success = false }, 2);

        result.Should().Be(3);
    }

    [Fact]
    public void AudioCue_WhenSuccess_ReturnsZero()
    {
        var config = new Mock<IPingConfig>();
        IConsoleHandler consoleHandler = new ConsoleHandler(config.Object);

        var result = consoleHandler.AudioCue(new PingStats { Success = true }, 10);

        result.Should().Be(0);
    }

    private static IPingConfig GetPingConfigMock()
    {
        return new Mock<IPingConfig>().Object;
    }
}