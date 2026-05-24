namespace Pinger.Interfaces;

public interface IPingTools
{
    string FormatElapsedTime(TimeSpan elapsed);
    long CalculateWorkDayPings(int snoozeTime, int workingHours);
}
