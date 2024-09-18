using Pinger.Interfaces;

namespace Pinger;

public class PingDisplay : IPingDisplay
{
    private IConsoleHandler ConsoleHandler { get; }
    private IPingConfig PingConfig { get; }

    public PingDisplay(IConsoleHandler consoleHandler, IPingConfig pingConfig)
    {
        ConsoleHandler = consoleHandler;
        PingConfig = pingConfig;

    }

    public void DisplayStatistics(decimal successRate, IPingStats status, string elapsed, ConsoleColor usual, IRollingStatistics rollingStatistics)
    {
        ConsoleHandler.WriteToConsole(
            $"{successRate}% R{status.PingTime}. T{rollingStatistics.TotalPings} P{rollingStatistics.SuccessfulPings} F{rollingStatistics.FailedPings}. A{rollingStatistics.AvgTime} S{rollingStatistics.Shortest} L{rollingStatistics.Longest} U{elapsed} C{rollingStatistics.StopAfterThisManyPings - rollingStatistics.TotalPings}");
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
        DisplayCodeName();
        ConsoleHandler.ForegroundColour = usual;
    }

    private void DisplayCodeName()
    {
        ConsoleHandler.ForegroundColour = ConsoleColor.Magenta;
        ConsoleHandler.WriteToConsole($"Code name: {PingConfig.CodeName}");
    }
}