using Pinger.Interfaces;
using static System.Console;

namespace Pinger.Domain;

public class ConsoleHandler : IConsoleHandler
{
    private IPingConfig PingConfig { get; }
    private long _failedPingsInCluster;

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
        get => ForegroundColor;
        set => ForegroundColor = value;
    }

    public virtual void Beep()
    {
        System.Console.Beep();
    }

    public void NotifyPingResult(IPingStats status)
    {
        if (status.Success)
        {
            _failedPingsInCluster = 0;
            return;
        }

        _failedPingsInCluster++;

        if (_failedPingsInCluster >= PingConfig.AlertAfterThisManyFailedPings)
        {
            Beep();
        }
    }
}
