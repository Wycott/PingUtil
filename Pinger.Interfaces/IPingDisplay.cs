namespace Pinger.Interfaces;

public interface IPingDisplay
{
    void DisplayStatistics(decimal successRate, IPingStats status, string elapsed, ConsoleColor usual,
        IRollingStatistics rollingStatistics);

    void SetDisplayColour(IPingStats status, decimal avgTime);

    void DisplaySettings(string remoteServer, int timeout, byte[] buffer, int snoozeTime, ConsoleColor usual,
        long stopAfterThisManyPings);
}