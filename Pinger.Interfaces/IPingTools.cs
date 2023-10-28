using System.Diagnostics;

namespace Pinger.Interfaces;

public interface IPingTools
{
    long CalculateWorkDayPings(int snoozeTime);
    string CalculateElapsedTime(Stopwatch sw);
}