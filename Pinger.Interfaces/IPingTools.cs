using System.Diagnostics;

namespace Pinger.Interfaces;

public interface IPingTools
{
    long CalculateWorkDayPings(int snoozeTime, int workingHours);
    string CalculateElapsedTime(Stopwatch sw);
}