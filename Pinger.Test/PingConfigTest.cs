using FluentAssertions;

namespace Pinger.Test;

public class PingConfigTest
{
    [Fact]
    public void WhenSetup_ThenItShouldBeAsExpected()
    {
        // Arrange
        const string expectedData = "When all else fails";
        const string expectedRemoteServer = "127.0.0.1";
        const int expectedSnoozeTime = 3000;
        const int expectedTimeout = 5000; ;
        const int expectedWorkingHours = 4;
        const bool expectedActiveFlag = false;

        // Act
        var pingConfig = new PingConfig()
        {
            Data = expectedData,
            RemoteServer = expectedRemoteServer,
            PingerIsActive = expectedActiveFlag,
            SnoozeTime = expectedSnoozeTime,
            Timeout = expectedTimeout,
            WorkingHours = expectedWorkingHours
        };

        // Assert
        pingConfig.Data.Should().Be(expectedData);
        pingConfig.RemoteServer.Should().Be(expectedRemoteServer);
        pingConfig.SnoozeTime.Should().Be(expectedSnoozeTime);
        pingConfig.Timeout.Should().Be(expectedTimeout);
        pingConfig.WorkingHours.Should().Be(expectedWorkingHours);
        pingConfig.PingerIsActive.Should().Be(expectedActiveFlag);
    }
}