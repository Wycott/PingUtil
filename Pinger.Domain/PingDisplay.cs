using Pinger.Interfaces;

namespace Pinger.Domain;

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
        var remaining = rollingStatistics.StopAfterThisManyPings - rollingStatistics.TotalPings;
        ConsoleHandler.WriteToConsole(
            $"{successRate}% Reply:{status.PingTime}ms Total:{rollingStatistics.TotalPings} Pass:{rollingStatistics.SuccessfulPings} Fail:{rollingStatistics.FailedPings} Avg:{rollingStatistics.AvgTime}ms Short:{rollingStatistics.Shortest}ms Long:{rollingStatistics.Longest}ms Up:{elapsed} Remaining:{remaining}");
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
