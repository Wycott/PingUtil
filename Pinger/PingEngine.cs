using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using Pinger.Interfaces;
using static System.Console;

namespace Pinger;

public class PingEngine : IPingEngine
{
    public void Start()
    {
        const string data = "All our lives we sweat and save.";
        const string remoteServer = "8.8.8.8";
        const int snoozeTime = 5000;
        const int timeout = 10000;

        var usual = ForegroundColor;
        var shortest = long.MaxValue;
        var longest = long.MinValue;

        decimal avgTime = 0;

        long successfulPings = 0;
        long failedPings = 0;
        long totalPings = 0;
        long totalTime = 0;
        long failedPingsInCluster = 0;

        var buffer = Encoding.ASCII.GetBytes(data);

        var stopAfterThisManyPings = CalculateWorkDayPings(snoozeTime);

        DisplaySettings(remoteServer, timeout, buffer, snoozeTime, usual, stopAfterThisManyPings);

        var sw = Stopwatch.StartNew();

        while (totalPings < stopAfterThisManyPings)
        {
            var status = PingHost(remoteServer, timeout, buffer);
            var successRate = UpdatePingStats(status, ref totalPings, ref successfulPings, ref failedPings, ref totalTime, ref avgTime, ref longest, ref shortest);
            SetDisplayColour(status, avgTime);
            failedPingsInCluster = AudioCue(status, failedPingsInCluster);
            var elapsed = CalculateElapsedTime(sw);
            DisplayStatistics(successRate, status, totalPings, successfulPings, failedPings, avgTime, shortest, longest, elapsed, stopAfterThisManyPings - totalPings, usual);

            Thread.Sleep(snoozeTime);
        }
    }

    private static long CalculateWorkDayPings(int snoozeTime)
    {
        const int workingHours = 16;

        var snoozeTimeInSeconds = snoozeTime / 1000;

        var pingsInADay = workingHours * 60 * 60 / snoozeTimeInSeconds;

        return pingsInADay;
    }

    private static void DisplayStatistics(decimal successRate, PingStats status, long totalPings, long successfulPings,
        long failedPings, decimal averageTime, long shortest, long longest, string elapsed, long remainingPings, ConsoleColor usual)
    {
        WriteLine(
            $"{successRate}% R{status.PingTime}. T{totalPings} P{successfulPings} F{failedPings}. A{averageTime} S{shortest} L{longest} U{elapsed} C{remainingPings}");
        ForegroundColor = usual;
    }

    private static string CalculateElapsedTime(Stopwatch sw)
    {
        var t = TimeSpan.FromSeconds(sw.ElapsedMilliseconds / 1000f);
        var hours = t.Hours;
        var minutes = t.Minutes;
        var seconds = t.Seconds;

        var elapsed = $"{hours:00}:{minutes:00}:{seconds:00}";

        return elapsed;
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

    private static void SetDisplayColour(PingStats status, decimal avgTime)
    {
        if (!status.Success)
        {
            ForegroundColor = ConsoleColor.Red;
        }

        if (status.PingTime > avgTime)
        {
            ForegroundColor = ConsoleColor.White;
        }
    }

    private static long AudioCue(PingStats status, long failedPingsInCluster)
    {
        const int beepAfter = 3;

        if (status.Success)
        {
            return 0;
        }

        failedPingsInCluster++;

        if (failedPingsInCluster >= beepAfter)
        {
            Beep();
        }

        return failedPingsInCluster;
    }

    private static void DisplaySettings(string remoteServer, int timeout, byte[] buffer, int snoozeTime, ConsoleColor usual, long stopAfterThisManyPings)
    {
        ForegroundColor = ConsoleColor.Yellow;
        WriteLine(
            $"Host: {remoteServer}, Timeout: {timeout}, Packet Size: {buffer.Length}, Snooze Time: {snoozeTime}, Data Points: {stopAfterThisManyPings}");
        ForegroundColor = usual;
    }

    private static PingStats PingHost(string nameOrAddress, int timeout, byte[] buffer)
    {
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