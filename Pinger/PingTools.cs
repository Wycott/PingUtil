using System.Diagnostics;
using Pinger.Interfaces;

namespace Pinger;

public class PingTools : IPingTools
{
    public long CalculateWorkDayPings(int snoozeTime)
    {
        const int workingHours = 16;

        var snoozeTimeInSeconds = snoozeTime / 1000;

        var pingsInADay = workingHours * 60 * 60 / snoozeTimeInSeconds;

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