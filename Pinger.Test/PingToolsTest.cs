using System.Diagnostics;
using FluentAssertions;
using Pinger.Domain;
using Pinger.Interfaces;

namespace Pinger.Test;

public class PingToolsTest
{
    [Fact]
    public void CalculateWorkDayPings()
    {
        // Arrange
        const int expectedDataPoints = 11520;
        IPingTools pingTools = new PingTools();

        // Act
        var res = pingTools.CalculateWorkDayPings(5000, 16);

        // Assert
        res.Should().Be(expectedDataPoints);
    }

    [Fact]
    public void CalculateElapsedTime()
    {
        // Arrange
        const string expectedResult = "00:00:00";

        IPingTools pingTools = new PingTools();
        var sw = new Stopwatch();
        sw.Start();
        sw.Stop();

        // Act
        var res = pingTools.CalculateElapsedTime(sw);

        // Assert
        res.Should().Be(expectedResult);
    }
}