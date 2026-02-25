using AiAnnotations;
using AiAnnotations.Types;
using FluentAssertions;
using Pinger.Domain;

namespace Pinger.Test;

[AiGenerated(Authorship.Ai)]
public class RollingStatisticsTest
{
    [Fact]
    public void WhenSetup_ThenItShouldBeAsExpected()
    {
        var stats = new RollingStatistics
        {
            StopAfterThisManyPings = 100,
            TotalPings = 50,
            SuccessfulPings = 45,
            FailedPings = 5,
            TotalTime = 1000,
            AvgTime = 22.2m,
            Longest = 50,
            Shortest = 10
        };

        stats.StopAfterThisManyPings.Should().Be(100);
        stats.TotalPings.Should().Be(50);
        stats.SuccessfulPings.Should().Be(45);
        stats.FailedPings.Should().Be(5);
        stats.TotalTime.Should().Be(1000);
        stats.AvgTime.Should().Be(22.2m);
        stats.Longest.Should().Be(50);
        stats.Shortest.Should().Be(10);
    }
}
