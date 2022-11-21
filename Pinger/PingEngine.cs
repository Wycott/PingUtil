using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;

namespace Pinger;

internal static class PingEngine
{
    internal static void Start()
    {
        const string Data = "All our lives we sweat and save.";
        const string RemoteServer = "8.8.8.8";
        const int SnoozeTime = 5000;
        const int Timeout = 10000;

        var usual = Console.ForegroundColor;
        var shortest = long.MaxValue;
        var longest = long.MinValue;

        decimal avgTime = 0;

        long successfulPings = 0;
        long failedPings = 0;
        long totalPings = 0;
        long totalTime = 0;

        var buffer = Encoding.ASCII.GetBytes(Data);

        DisplaySettings(RemoteServer, Timeout, buffer, SnoozeTime, usual);
        var stopAfterThisManyPings = CalculateWorkDayPings(SnoozeTime);

        var sw = Stopwatch.StartNew();

        while (totalPings < stopAfterThisManyPings)
        {
            var status = PingHost(RemoteServer, Timeout, buffer);
            var successRate = UpdatePingStats(status, ref totalPings, ref successfulPings, ref failedPings, ref totalTime, ref avgTime, ref longest, ref shortest);
            SetDisplayColour(status, avgTime);
            var elapsed = CalculateElapsedTime(sw);
            DisplayStatistics(successRate, status, totalPings, successfulPings, failedPings, avgTime, shortest, longest, elapsed, stopAfterThisManyPings - totalPings, usual);

            Thread.Sleep(SnoozeTime);
        }
    }

    private static long CalculateWorkDayPings(int snoozeTime)
    {
        const int WorkingHours = 16;

        var snoozeTimeInSeconds = snoozeTime / 1000;

        var pingsInADay = (WorkingHours * 60 * 60) / snoozeTimeInSeconds;

        return pingsInADay;

    }

    private static void DisplayStatistics(decimal successRate, PingStats status, long totalPings, long successfulPings,
        long failedPings, decimal avgTime, long shortest, long longest, string elapsed, long remainingPings, ConsoleColor usual)
    {
        Console.WriteLine(
            $"{successRate}% R{status.PingTime}. T{totalPings} P{successfulPings} F{failedPings}. A{avgTime} S{shortest} L{longest} U{elapsed} C{remainingPings}");
        Console.ForegroundColor = usual;
    }

    private static string CalculateElapsedTime(Stopwatch sw)
    {
        // ReSharper disable once PossibleLossOfFraction
        var t = TimeSpan.FromSeconds(sw.ElapsedMilliseconds / 1000);
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
            Console.ForegroundColor = ConsoleColor.Red;
        }

        if (status.PingTime > avgTime)
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    private static void DisplaySettings(string remoteServer, int timeout, byte[] buffer, int snoozeTime, ConsoleColor usual)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(
            $"Host: {remoteServer}, Timeout: {timeout}, Packet Size: {buffer.Length}, Snooze Time: {snoozeTime}");
        Console.ForegroundColor = usual;
    }

    private static PingStats PingHost(string nameOrAddress, int timeout, byte[] buffer)
    {
        var pinger = new Ping();

        try
        {
            var reply = pinger.Send(nameOrAddress, timeout, buffer);
            var ps = new PingStats
            {
                Success = reply.Status == IPStatus.Success,
                PingTime = reply.RoundtripTime
            };

            return ps;
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