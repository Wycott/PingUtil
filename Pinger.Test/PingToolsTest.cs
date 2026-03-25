using AiAnnotations;
using AiAnnotations.Types;
using FluentAssertions;
using Pinger.Domain;
using Pinger.Interfaces;
using System.Diagnostics;

namespace Pinger.Test;

[AiGenerated(Authorship.Hybrid)]
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

    [Fact]
    public void CalculateWorkDayPings_WithDifferentValues()
    {
        IPingTools pingTools = new PingTools();

        var result = pingTools.CalculateWorkDayPings(1000, 1);

        result.Should().Be(3600);
    }

    [Fact]
    public void CalculateElapsedTime_WithLongerDuration()
    {
        IPingTools pingTools = new PingTools();
        var sw = new Stopwatch();
        sw.Start();
        Thread.Sleep(1000);
        sw.Stop();

        var result = pingTools.CalculateElapsedTime(sw);

        result.Should().MatchRegex(@"\d{2}:\d{2}:\d{2}");
    }
}
