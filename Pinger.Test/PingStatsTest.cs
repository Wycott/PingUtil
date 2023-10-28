using FluentAssertions;

namespace Pinger.Test;

public class PingStatsTest
{
    [Fact]
    public void WhenSetup_ThenItShouldBeAsExpected()
    {
        // Arrange
        const int expectedPingTime = 20;
        const bool expectedSuccessFlag = true;

        // Act
        var ps = new PingStats { PingTime = expectedPingTime, Success = expectedSuccessFlag };

        // Assert
        ps.PingTime.Should().Be(expectedPingTime);
        ps.Success.Should().Be(expectedSuccessFlag);
    }
}