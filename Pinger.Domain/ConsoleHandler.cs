using Pinger.Interfaces;
using static System.Console;

namespace Pinger.Domain;

public class ConsoleHandler : IConsoleHandler
{
    private IPingConfig PingConfig { get; set; }

    public ConsoleHandler(IPingConfig pingConfig)
    {
        PingConfig = pingConfig;
    }

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
        var beepAfter = PingConfig.AlertAfterThisManyFailedPings;

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