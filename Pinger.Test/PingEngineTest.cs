using AiAnnotations;
using AiAnnotations.Types;
using FluentAssertions;
using Moq;
using Pinger.Domain;
using Pinger.Interfaces;

namespace Pinger.Test;

[AiGenerated(Authorship.Hybrid)]
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
        var mockRollingStatistics = new Mock<IRollingStatistics>();
        IPingEngine pingEngine = new PingEngine(mockPingToolsMock.Object, mockPingDisplayMock.Object,
            mockConsoleHandler.Object, mockPingConfig.Object, mockRollingStatistics.Object);

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
        var mockRollingStatistics = new Mock<IRollingStatistics>();
        mockPingConfig.Setup(x => x.Data).Returns("Carrying babies to the river");
        mockPingConfig.Setup(x => x.PingerIsActive).Returns(false);
        mockPingConfig.Setup(x => x.WorkingHours).Returns(1);
        IPingEngine pingEngine = new PingEngine(mockPingToolsMock.Object, mockPingDisplayMock.Object,
            mockConsoleHandler.Object, mockPingConfig.Object, mockRollingStatistics.Object);
        pingEngine.Start();

        // Assert
        pingEngine.Should().NotBeNull();
    }

    [Fact]
    public void Start_CallsDisplaySettings()
    {
        var mockPingTools = new Mock<IPingTools>();
        mockPingTools.Setup(x => x.CalculateWorkDayPings(It.IsAny<int>(), It.IsAny<int>())).Returns(1);
        var mockPingDisplay = new Mock<IPingDisplay>();
        var mockConsoleHandler = new Mock<IConsoleHandler>();
        var mockPingConfig = new Mock<IPingConfig>();
        mockPingConfig.Setup(x => x.Data).Returns("test");
        mockPingConfig.Setup(x => x.PingerIsActive).Returns(false);
        var mockRollingStatistics = new Mock<IRollingStatistics>();

        IPingEngine pingEngine = new PingEngine(mockPingTools.Object, mockPingDisplay.Object,
            mockConsoleHandler.Object, mockPingConfig.Object, mockRollingStatistics.Object);
        pingEngine.Start();

        mockPingDisplay.Verify(x => x.DisplaySettings(It.IsAny<string>(), It.IsAny<int>(), 
            It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<ConsoleColor>(), It.IsAny<long>()), Times.Once);
    }
}
