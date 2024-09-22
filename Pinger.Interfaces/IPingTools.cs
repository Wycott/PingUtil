using System.Diagnostics;

namespace Pinger.Interfaces;

public interface IPingTools
{
    string CalculateElapsedTime(Stopwatch sw);
    long CalculateWorkDayPings(int snoozeTime, int workingHours);
}