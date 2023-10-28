using Pinger.Interfaces;
using FluentAssertions;
using Moq;

namespace Pinger.Test;

public class PingEngineTest
{
    [Fact]
    public void Constructor()
    {
        // Arrange/Act
        var mockPingToolsMock = new Mock<IPingTools>();
        var mockPingDisplayMock = new Mock<IPingDisplay>();
        var mockConsoleHandler = new Mock<IConsoleHandler>();
        var mockPingConfig = new Mock<IPingConfig>();
        IPingEngine pingEngine = new PingEngine(mockPingToolsMock.Object, mockPingDisplayMock.Object,
            mockConsoleHandler.Object, mockPingConfig.Object);

        // Assert
        pingEngine.Should().NotBeNull();

    }

    [Fact]
    public void Start()
    {
        // Arrange/Act
        var mockPingToolsMock = new Mock<IPingTools>();
        mockPingToolsMock.Setup(x => x.CalculateWorkDayPings(It.IsAny<int>(), It.IsAny<int>())).Returns(10);
        var mockPingDisplayMock = new Mock<IPingDisplay>();
        var mockConsoleHandler = new Mock<IConsoleHandler>();
        var mockPingConfig = new Mock<IPingConfig>();
        mockPingConfig.Setup(x => x.Data).Returns("Carrying babies to the river");
        mockPingConfig.Setup(x => x.PingerIsActive).Returns(false);
        mockPingConfig.Setup(x => x.WorkingHours).Returns(1);
        IPingEngine pingEngine = new PingEngine(mockPingToolsMock.Object, mockPingDisplayMock.Object,
            mockConsoleHandler.Object, mockPingConfig.Object);
        pingEngine.Start();

        // Assert
        pingEngine.Should().NotBeNull();
    }
}