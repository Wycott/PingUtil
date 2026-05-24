using FluentAssertions;
using Pinger.Domain;
using Pinger.Interfaces;

namespace Pinger.Test;

public class PingToolsTest
{
    [Fact]
    public void CalculateWorkDayPings_16Hours5SecondSnooze()
    {
        IPingTools pingTools = new PingTools();

        var res = pingTools.CalculateWorkDayPings(5000, 16);

        res.Should().Be(11520);
    }

    [Fact]
    public void CalculateWorkDayPings_1Hour1SecondSnooze()
    {
        IPingTools pingTools = new PingTools();

        var result = pingTools.CalculateWorkDayPings(1000, 1);

        result.Should().Be(3600);
    }

    [Fact]
    public void CalculateWorkDayPings_SmallSnoozeTime()
    {
        IPingTools pingTools = new PingTools();

        // 2000ms snooze, 1 hour = 1800 pings
        var result = pingTools.CalculateWorkDayPings(2000, 1);

        result.Should().Be(1800);
    }

    [Fact]
    public void FormatElapsedTime_Zero()
    {
        IPingTools pingTools = new PingTools();

        var res = pingTools.FormatElapsedTime(TimeSpan.Zero);

        res.Should().Be("00:00:00");
    }

    [Fact]
    public void FormatElapsedTime_OneHourOneMinuteOneSecond()
    {
        IPingTools pingTools = new PingTools();

        var result = pingTools.FormatElapsedTime(TimeSpan.FromSeconds(3661));

        result.Should().Be("01:01:01");
    }

    [Fact]
    public void FormatElapsedTime_Over24Hours_DoesNotWrap()
    {
        IPingTools pingTools = new PingTools();

        var result = pingTools.FormatElapsedTime(TimeSpan.FromHours(25.5));

        result.Should().Be("25:30:00");
    }

    [Fact]
    public void FormatElapsedTime_JustUnderAnHour()
    {
        IPingTools pingTools = new PingTools();

        var result = pingTools.FormatElapsedTime(TimeSpan.FromMinutes(59).Add(TimeSpan.FromSeconds(59)));

        result.Should().Be("00:59:59");
    }
}
