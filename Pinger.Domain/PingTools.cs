using AiAnnotations;
using AiAnnotations.Types;
using Pinger.Interfaces;
using System.Diagnostics;

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

    [AiGenerated(Authorship.Hybrid)]
    public string CalculateElapsedTime(Stopwatch sw)
    {
        var elapsed = sw.Elapsed;

        return $"{elapsed.Hours:00}:{elapsed.Minutes:00}:{elapsed.Seconds:00}";
    }
}