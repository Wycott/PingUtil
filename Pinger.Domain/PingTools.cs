using System.Diagnostics;
using Pinger.Interfaces;

namespace Pinger.Domain;

public class PingTools : IPingTools
{
    public long CalculateWorkDayPings(int snoozeTime, int workingHours)
    {
        const int millisecondsInSecond = 1000;
        const int minutesInHour = 60;
        const int secondsInMinute = 60;

        var snoozeTimeInSeconds = snoozeTime / millisecondsInSecond;

        var pingsInADay = workingHours * minutesInHour * secondsInMinute / snoozeTimeInSeconds;

        return pingsInADay;
    }

    public string CalculateElapsedTime(Stopwatch sw)
    {
        var t = TimeSpan.FromSeconds(sw.ElapsedMilliseconds / 1000f);
        var hours = t.Hours;
        var minutes = t.Minutes;
        var seconds = t.Seconds;

        var elapsed = $"{hours:00}:{minutes:00}:{seconds:00}";

        return elapsed;
    }
}