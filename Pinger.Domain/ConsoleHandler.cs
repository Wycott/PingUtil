using Pinger.Interfaces;
using static System.Console;

namespace Pinger.Domain;

public class ConsoleHandler(IPingConfig pingConfig) : IConsoleHandler
{
    private IPingConfig PingConfig { get; } = pingConfig;
    private long failedPingsInCluster;

    public void WriteToConsole(string message) => WriteLine(message);

    public ConsoleColor ForegroundColour
    {
        get => ForegroundColor;
        set => ForegroundColor = value;
    }

    protected virtual void Beep() => System.Console.Beep();

    public void NotifyPingResult(IPingStats status)
    {
        if (status.Success)
        {
            failedPingsInCluster = 0;

            return;
        }

        failedPingsInCluster++;

        if (failedPingsInCluster >= PingConfig.AlertAfterThisManyFailedPings)
        {
            Beep();
        }
    }
}
