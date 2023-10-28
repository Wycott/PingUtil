using Pinger.Interfaces;

namespace Pinger;

public class PingDisplay : IPingDisplay
{
    private IConsoleHandler ConsoleHandler { get; }

    public PingDisplay(IConsoleHandler consoleHandler)
    {
        ConsoleHandler = consoleHandler;
    }

    public void DisplayStatistics(decimal successRate, IPingStats status, long totalPings, long successfulPings, long failedPings,
        decimal averageTime, long shortest, long longest, string elapsed, long remainingPings, ConsoleColor usual)
    {
        ConsoleHandler.WriteToConsole(
            $"{successRate}% R{status.PingTime}. T{totalPings} P{successfulPings} F{failedPings}. A{averageTime} S{shortest} L{longest} U{elapsed} C{remainingPings}");
        ConsoleHandler.ForegroundColour = usual;
    }

    public void SetDisplayColour(IPingStats status, decimal avgTime)
    {
        if (!status.Success)
        {
            ConsoleHandler.ForegroundColour = ConsoleColor.Red;
        }

        if (status.PingTime > avgTime)
        {
            ConsoleHandler.ForegroundColour = ConsoleColor.White;
        }
    }

    public void DisplaySettings(string remoteServer, int timeout, byte[] buffer, int snoozeTime, ConsoleColor usual, long stopAfterThisManyPings)
    {
        ConsoleHandler.ForegroundColour = ConsoleColor.Yellow;
        ConsoleHandler.WriteToConsole(
            $"Host: {remoteServer}, Timeout: {timeout}, Packet Size: {buffer.Length}, Snooze Time: {snoozeTime}, Data Points: {stopAfterThisManyPings}");
        ConsoleHandler.ForegroundColour = usual;
    }
}