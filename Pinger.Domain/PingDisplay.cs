using Pinger.Interfaces;

namespace Pinger.Domain;

public class PingDisplay : IPingDisplay
{
    private IConsoleHandler ConsoleHandler { get; }
    private int SnoozeTime { get; }
    private string CodeName { get; }

    public PingDisplay(IConsoleHandler consoleHandler, IPingConfig pingConfig)
    {
        ConsoleHandler = consoleHandler;
        SnoozeTime = pingConfig.SnoozeTime;
        CodeName = pingConfig.CodeName;
    }

    public void DisplayStatistics(decimal successRate, IPingStats status, string elapsed, ConsoleColor usual, IRollingStatistics rollingStatistics)
    {
        var remaining = rollingStatistics.StopAfterThisManyPings - rollingStatistics.TotalPings;
        var estimatedEnd = CalculateCountdown(remaining);
        var shortest = FormatShortest(rollingStatistics.Shortest);
        ConsoleHandler.WriteToConsole(
            $"{successRate}% Reply:{status.PingTime}ms Total:{rollingStatistics.TotalPings} Pass:{rollingStatistics.SuccessfulPings} Fail:{rollingStatistics.FailedPings} Avg:{rollingStatistics.AvgTime}ms Short:{shortest}ms Long:{rollingStatistics.Longest}ms Up:{elapsed} Remaining:{remaining} ({estimatedEnd})");
        ConsoleHandler.ForegroundColour = usual;
    }

    public void DisplaySummary(string elapsed, ConsoleColor usual, IRollingStatistics rollingStatistics)
    {
        var shortest = FormatShortest(rollingStatistics.Shortest);
        ConsoleHandler.ForegroundColour = ConsoleColor.Yellow;
        ConsoleHandler.WriteToConsole(
            $"--- Session complete. Total:{rollingStatistics.TotalPings} Pass:{rollingStatistics.SuccessfulPings} Fail:{rollingStatistics.FailedPings} Avg:{rollingStatistics.AvgTime}ms Short:{shortest}ms Long:{rollingStatistics.Longest}ms Duration:{elapsed} ---");
        ConsoleHandler.ForegroundColour = usual;
    }

    private static string FormatShortest(long shortest)
    {
        return shortest == long.MaxValue ? "0" : shortest.ToString();
    }

    private string CalculateCountdown(long remainingPings)
    {
        var millisecondsRemaining = remainingPings * SnoozeTime;
        var timeRemaining = TimeSpan.FromMilliseconds(millisecondsRemaining);
        return $"{(int)timeRemaining.TotalHours:00}:{timeRemaining.Minutes:00}:{timeRemaining.Seconds:00}";
    }

    public void SetDisplayColour(IPingStats status, decimal avgTime, ConsoleColor usual)
    {
        if (!status.Success)
        {
            ConsoleHandler.ForegroundColour = ConsoleColor.Red;
        }
        else if (status.PingTime > avgTime)
        {
            ConsoleHandler.ForegroundColour = ConsoleColor.White;
        }
        else
        {
            ConsoleHandler.ForegroundColour = usual;
        }
    }

    public void DisplaySettings(string remoteServer, int timeout, byte[] buffer, ConsoleColor usual, long stopAfterThisManyPings)
    {
        ConsoleHandler.ForegroundColour = ConsoleColor.Yellow;
        ConsoleHandler.WriteToConsole(
            $"Host: {remoteServer}, Timeout: {timeout}, Packet Size: {buffer.Length}, Snooze Time: {SnoozeTime}, Data Points: {stopAfterThisManyPings}");
        DisplayCodeName();
        ConsoleHandler.ForegroundColour = usual;
    }

    private void DisplayCodeName()
    {
        ConsoleHandler.ForegroundColour = ConsoleColor.Magenta;
        ConsoleHandler.WriteToConsole($"Code name: {CodeName}");
    }
}
