namespace Pinger.Interfaces;

public interface IPingDisplay
{
    void DisplayStatistics(decimal successRate, IPingStats status, long totalPings, long successfulPings,
        long failedPings, decimal averageTime, long shortest, long longest, string elapsed, long remainingPings,
        ConsoleColor usual);

    void SetDisplayColour(IPingStats status, decimal avgTime);

    void DisplaySettings(string remoteServer, int timeout, byte[] buffer, int snoozeTime, ConsoleColor usual,
        long stopAfterThisManyPings);
}