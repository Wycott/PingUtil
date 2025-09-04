using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using Pinger.Interfaces;
using static System.Console;

namespace Pinger.Domain;

public class PingEngine : IPingEngine
{
    private IPingTools PingToolKit { get; }
    private IPingDisplay PingDisplay { get; }
    private IConsoleHandler ConsoleHandler { get; }
    private IPingConfig PingConfig { get; }
    private IRollingStatistics RollingStatistics { get; }

    public PingEngine(IPingTools pingTools, IPingDisplay pingDisplay, IConsoleHandler consoleHandler, IPingConfig pingConfig, IRollingStatistics rollingStatistics)
    {
        PingToolKit = pingTools;
        PingDisplay = pingDisplay;
        ConsoleHandler = consoleHandler;
        PingConfig = pingConfig;
        RollingStatistics = rollingStatistics;
    }

    public void Start()
    {
        var usual = ForegroundColor;

        long failedPingsInCluster = 0;

        var buffer = Encoding.ASCII.GetBytes(PingConfig.Data);

        RollingStatistics.StopAfterThisManyPings = PingToolKit.CalculateWorkDayPings(PingConfig.SnoozeTime, PingConfig.WorkingHours);

        PingDisplay.DisplaySettings(PingConfig.RemoteServer, PingConfig.Timeout, buffer, PingConfig.SnoozeTime, usual, RollingStatistics.StopAfterThisManyPings);

        var sw = Stopwatch.StartNew();

        while (RollingStatistics.TotalPings < RollingStatistics.StopAfterThisManyPings)
        {
            failedPingsInCluster = PerformPingUpdateAndDisplayStatistics(buffer, failedPingsInCluster, sw, usual);
        }
    }

    private long PerformPingUpdateAndDisplayStatistics(byte[] buffer, long failedPingsInCluster, Stopwatch sw,
        ConsoleColor usual)
    {
        var status = PingHost(PingConfig.RemoteServer, PingConfig.Timeout, buffer);
        var successRate = UpdatePingStats(status, RollingStatistics);
        PingDisplay.SetDisplayColour(status, RollingStatistics.AvgTime);
        failedPingsInCluster = ConsoleHandler.AudioCue(status, failedPingsInCluster);
        var elapsed = PingToolKit.CalculateElapsedTime(sw);
        PingDisplay.DisplayStatistics(successRate, status, elapsed, usual, RollingStatistics);
        Thread.Sleep(PingConfig.SnoozeTime);

        return failedPingsInCluster;
    }

    private static decimal UpdatePingStats(PingStats status, IRollingStatistics rollingStatistics)
    {
        rollingStatistics.TotalPings++;

        UpdatePassFailStats(status, rollingStatistics);

        if (status.Success)
        {
            UpdateGeneralStats(status, rollingStatistics);
        }

        return Math.Round(rollingStatistics.SuccessfulPings / (decimal)rollingStatistics.TotalPings * 100, 1);
    }

    private static void UpdateGeneralStats(PingStats status, IRollingStatistics rollingStatistics)
    {
        rollingStatistics.TotalTime += status.PingTime;
        rollingStatistics.AvgTime = Math.Round(rollingStatistics.TotalTime / (decimal)rollingStatistics.SuccessfulPings, 1);

        if (status.PingTime > rollingStatistics.Longest)
        {
            rollingStatistics.Longest = status.PingTime;
        }

        if (status.PingTime < rollingStatistics.Shortest)
        {
            rollingStatistics.Shortest = status.PingTime;
        }
    }

    private static void UpdatePassFailStats(PingStats status, IRollingStatistics rollingStatistics)
    {
        if (status.Success)
        {
            rollingStatistics.SuccessfulPings++;
        }
        else
        {
            rollingStatistics.FailedPings++;
        }
    }

    private PingStats PingHost(string nameOrAddress, int timeout, byte[] buffer)
    {
        if (!PingConfig.PingerIsActive)
        {
            return new PingStats() { Success = true };
        }

        var pinger = new Ping();

        try
        {
            var reply = pinger.Send(nameOrAddress, timeout, buffer);
            var pingStats = new PingStats
            {
                Success = reply.Status == IPStatus.Success,
                PingTime = reply.RoundtripTime
            };

            return pingStats;
        }
        catch (PingException)
        {
            // Don't care what type of failure it is
        }
        finally
        {
            pinger.Dispose();
        }

        return new PingStats();
    }
}