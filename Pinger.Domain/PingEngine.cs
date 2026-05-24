using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using Pinger.Interfaces;

namespace Pinger.Domain;

public class PingEngine : IPingEngine
{
    private IPingTools PingTools { get; }
    private IPingDisplay PingDisplay { get; }
    private IConsoleHandler ConsoleHandler { get; }
    private IPingConfig PingConfig { get; }
    private IRollingStatistics RollingStatistics { get; }

    public PingEngine(IPingTools pingTools, IPingDisplay pingDisplay, IConsoleHandler consoleHandler, IPingConfig pingConfig, IRollingStatistics rollingStatistics)
    {
        PingTools = pingTools;
        PingDisplay = pingDisplay;
        ConsoleHandler = consoleHandler;
        PingConfig = pingConfig;
        RollingStatistics = rollingStatistics;
    }

    public void Start()
    {
        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        StartAsync(cts.Token).GetAwaiter().GetResult();
    }

    private async Task StartAsync(CancellationToken cancellationToken)
    {
        var usual = Console.ForegroundColor;

        var buffer = Encoding.ASCII.GetBytes(PingConfig.Data);

        RollingStatistics.StopAfterThisManyPings = PingTools.CalculateWorkDayPings(PingConfig.SnoozeTime, PingConfig.WorkingHours);

        PingDisplay.DisplaySettings(PingConfig.RemoteServer, PingConfig.Timeout, buffer, PingConfig.SnoozeTime, usual, RollingStatistics.StopAfterThisManyPings);

        var sw = Stopwatch.StartNew();

        while (RollingStatistics.TotalPings < RollingStatistics.StopAfterThisManyPings && !cancellationToken.IsCancellationRequested)
        {
            PerformPingUpdateAndDisplayStatistics(buffer, sw, usual);

            try
            {
                await Task.Delay(PingConfig.SnoozeTime, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }
    }

    private void PerformPingUpdateAndDisplayStatistics(byte[] buffer, Stopwatch sw, ConsoleColor usual)
    {
        var status = PingHost(PingConfig.RemoteServer, PingConfig.Timeout, buffer);
        var successRate = RollingStatistics.RecordPing(status);
        PingDisplay.SetDisplayColour(status, RollingStatistics.AvgTime, usual);
        ConsoleHandler.NotifyPingResult(status);
        var elapsed = PingTools.FormatElapsedTime(sw.Elapsed);
        PingDisplay.DisplayStatistics(successRate, status, elapsed, usual, RollingStatistics);
    }

    private PingStats PingHost(string nameOrAddress, int timeout, byte[] buffer)
    {
        if (!PingConfig.PingerIsActive)
        {
            return new PingStats() { Success = true };
        }

        try
        {
            using var pinger = new Ping();
            var reply = pinger.Send(nameOrAddress, timeout, buffer);
            return new PingStats
            {
                Success = reply.Status == IPStatus.Success,
                PingTime = reply.RoundtripTime
            };
        }
        catch (Exception)
        {
            // Treat any ping failure as a failed ping
            return new PingStats();
        }
    }
}
