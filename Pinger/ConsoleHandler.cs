using Pinger.Interfaces;
using static System.Console;

namespace Pinger;

public class ConsoleHandler : IConsoleHandler
{
    public void WriteToConsole(string message)
    {
        WriteLine(message);                
    }

    public ConsoleColor ForegroundColour
    {
        get => Console.ForegroundColor;
        set => ForegroundColor = value;
    }

    public long AudioCue(IPingStats status, long failedPingsInCluster)
    {
        const int beepAfter = 3;

        if (status.Success)
        {
            return 0;
        }

        failedPingsInCluster++;

        if (failedPingsInCluster >= beepAfter)
        {
            Beep();
        }

        return failedPingsInCluster;
    }
}