using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using Pinger.Interfaces;
using static System.Console;

namespace Pinger;

public class PingEngine : IPingEngine
{
    private IPingTools PingToolKit { get; }
    private IPingDisplay PingDisplay { get; }
    private IConsoleHandler ConsoleHandler { get; }
    private IPingConfig PingConfig { get; }

    public PingEngine(IPingTools pingTools, IPingDisplay pingDisplay, IConsoleHandler consoleHandler, IPingConfig pingConfig)
    {
        PingToolKit = pingTools;
        PingDisplay = pingDisplay;
        ConsoleHandler = consoleHandler;
        PingConfig = pingConfig;
    }

    public void Start()
    {
        var usual = ForegroundColor;
        var shortest = long.MaxValue;
        var longest = long.MinValue;

        decimal avgTime = 0;

        long successfulPings = 0;
        long failedPings = 0;
        long totalPings = 0;
        long totalTime = 0;
        long failedPingsInCluster = 0;

        var buffer = Encoding.ASCII.GetBytes(PingConfig.Data);

        var stopAfterThisManyPings = PingToolKit.CalculateWorkDayPings(PingConfig.SnoozeTime, PingConfig.WorkingHours);

        PingDisplay.DisplaySettings(PingConfig.RemoteServer, PingConfig.Timeout, buffer, PingConfig.SnoozeTime, usual, stopAfterThisManyPings);

        var sw = Stopwatch.StartNew();

        while (totalPings < stopAfterThisManyPings)
        {
            var status = PingHost(PingConfig.RemoteServer, PingConfig.Timeout, buffer);
            var successRate = UpdatePingStats(status, ref totalPings, ref successfulPings, ref failedPings, ref totalTime, ref avgTime, ref longest, ref shortest);
            PingDisplay.SetDisplayColour(status, avgTime);
            failedPingsInCluster = ConsoleHandler.AudioCue(status, failedPingsInCluster);
            var elapsed = PingToolKit.CalculateElapsedTime(sw);
            PingDisplay.DisplayStatistics(successRate, status, totalPings, successfulPings, failedPings, avgTime, shortest, longest, elapsed, stopAfterThisManyPings - totalPings, usual);

            Thread.Sleep(PingConfig.SnoozeTime);
        }
    }

    private static decimal UpdatePingStats(PingStats status, ref long totalPings, ref long successfulPings,
        ref long failedPings, ref long totalTime, ref decimal avgTime, ref long longest, ref long shortest)
    {
        totalPings++;

        if (status.Success)
        {
            successfulPings++;
        }
        else
        {
            failedPings++;
        }

        if (status.Success)
        {
            totalTime += status.PingTime;
            avgTime = Math.Round(totalTime / (decimal)successfulPings, 1);

            if (status.PingTime > longest)
            {
                longest = status.PingTime;
            }

            if (status.PingTime < shortest)
            {
                shortest = status.PingTime;
            }
        }

        var successRate = Math.Round(successfulPings / (decimal)totalPings * 100, 1);

        return successRate;
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