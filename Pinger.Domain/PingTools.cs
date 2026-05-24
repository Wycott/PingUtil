using Pinger.Interfaces;

namespace Pinger.Domain;

public class PingTools : IPingTools
{
    public long CalculateWorkDayPings(int snoozeTime, int workingHours)
    {
        if (snoozeTime <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(snoozeTime), "Snooze time must be greater than zero.");
        }

        const int millisecondsInHour = 3_600_000;

        var totalMilliseconds = (long)workingHours * millisecondsInHour;

        return totalMilliseconds / snoozeTime;
    }

    public string FormatElapsedTime(TimeSpan elapsed)
    {
        return $"{(int)elapsed.TotalHours:00}:{elapsed.Minutes:00}:{elapsed.Seconds:00}";
    }
}
