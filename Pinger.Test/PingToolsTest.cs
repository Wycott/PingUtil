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
    public void CalculateWorkDayPings_SubSecondSnooze()
    {
        IPingTools pingTools = new PingTools();

        // 500ms snooze, 1 hour = 7200 pings
        var result = pingTools.CalculateWorkDayPings(500, 1);

        result.Should().Be(7200);
    }

    [Fact]
    public void CalculateWorkDayPings_ZeroSnooze_ThrowsArgumentOutOfRange()
    {
        IPingTools pingTools = new PingTools();

        var act = () => pingTools.CalculateWorkDayPings(0, 1);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void CalculateWorkDayPings_NegativeSnooze_ThrowsArgumentOutOfRange()
    {
        IPingTools pingTools = new PingTools();

        var act = () => pingTools.CalculateWorkDayPings(-1000, 1);

        act.Should().Throw<ArgumentOutOfRangeException>();
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
