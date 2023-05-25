namespace Pinger.Test
{
    public class PingStatsTest
    {
        [Fact]
        public void WhenSetup_ThenShouldBeAsExpected()
        {
            // Arrange
            const int ExpectedPingTime = 20;
            const bool ExpectedSuccessFlag = true;

            // Act
            var ps = new PingStats() { PingTime = ExpectedPingTime, Success = ExpectedSuccessFlag };

            // Assert
            Assert.Equal(ExpectedPingTime, ps.PingTime);
            Assert.Equal(ExpectedSuccessFlag, ps.Success);
        }
    }
}