namespace Pinger.Interfaces;

public interface IPingDisplay
{
    void DisplayStatistics(decimal successRate, IPingStats status, string elapsed, ConsoleColor usual,
        IRollingStatistics rollingStatistics);

    void SetDisplayColour(IPingStats status, decimal avgTime, ConsoleColor usual);

    void DisplaySettings(string remoteServer, int timeout, byte[] buffer, ConsoleColor usual,
        long stopAfterThisManyPings);

    void DisplaySummary(string elapsed, ConsoleColor usual, IRollingStatistics rollingStatistics);
}
