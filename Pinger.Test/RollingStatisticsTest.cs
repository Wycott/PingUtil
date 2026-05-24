using FluentAssertions;
using Pinger.Domain;

namespace Pinger.Test;

public class RollingStatisticsTest
{
    [Fact]
    public void RecordPing_WhenSuccess_UpdatesStats()
    {
        var stats = new RollingStatistics();

        stats.RecordPing(new PingStats { Success = true, PingTime = 20 });
        stats.RecordPing(new PingStats { Success = true, PingTime = 30 });

        stats.TotalPings.Should().Be(2);
        stats.SuccessfulPings.Should().Be(2);
        stats.FailedPings.Should().Be(0);
        stats.TotalTime.Should().Be(50);
        stats.AvgTime.Should().Be(25);
        stats.Shortest.Should().Be(20);
        stats.Longest.Should().Be(30);
    }

    [Fact]
    public void RecordPing_WhenFailed_UpdatesFailCount()
    {
        var stats = new RollingStatistics();

        stats.RecordPing(new PingStats { Success = false });

        stats.TotalPings.Should().Be(1);
        stats.SuccessfulPings.Should().Be(0);
        stats.FailedPings.Should().Be(1);
    }

    [Fact]
    public void RecordPing_ReturnsSuccessRate()
    {
        var stats = new RollingStatistics();

        stats.RecordPing(new PingStats { Success = true, PingTime = 10 });
        stats.RecordPing(new PingStats { Success = true, PingTime = 10 });
        var rate = stats.RecordPing(new PingStats { Success = false });

        rate.Should().Be(66.7m);
    }

    [Fact]
    public void RecordPing_AllSuccess_Returns100Percent()
    {
        var stats = new RollingStatistics();

        var rate = stats.RecordPing(new PingStats { Success = true, PingTime = 15 });

        rate.Should().Be(100m);
    }

    [Fact]
    public void RecordPing_ShortestDoesNotChangeWhenLaterPingIsLonger()
    {
        var stats = new RollingStatistics();

        stats.RecordPing(new PingStats { Success = true, PingTime = 10 });
        stats.RecordPing(new PingStats { Success = true, PingTime = 50 });

        stats.Shortest.Should().Be(10);
    }

    [Fact]
    public void RecordPing_LongestDoesNotChangeWhenLaterPingIsShorter()
    {
        var stats = new RollingStatistics();

        stats.RecordPing(new PingStats { Success = true, PingTime = 50 });
        stats.RecordPing(new PingStats { Success = true, PingTime = 10 });

        stats.Longest.Should().Be(50);
    }

    [Fact]
    public void RecordPing_FailedPingsDoNotAffectTimeStats()
    {
        var stats = new RollingStatistics();

        stats.RecordPing(new PingStats { Success = true, PingTime = 20 });
        stats.RecordPing(new PingStats { Success = false, PingTime = 999 });

        stats.TotalTime.Should().Be(20);
        stats.AvgTime.Should().Be(20);
        stats.Longest.Should().Be(20);
        stats.Shortest.Should().Be(20);
    }

    [Fact]
    public void StopAfterThisManyPings_IsSettable()
    {
        var stats = new RollingStatistics { StopAfterThisManyPings = 100 };

        stats.StopAfterThisManyPings.Should().Be(100);
    }

    [Fact]
    public void Shortest_DefaultsToMaxValue()
    {
        var stats = new RollingStatistics();

        stats.Shortest.Should().Be(long.MaxValue);
    }
}
