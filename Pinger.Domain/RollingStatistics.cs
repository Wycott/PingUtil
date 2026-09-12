using Pinger.Interfaces;

namespace Pinger.Domain;

public class RollingStatistics : IRollingStatistics
{
    public long StopAfterThisManyPings { get; set; }
    public long TotalPings { get; private set; }
    public long SuccessfulPings { get; private set; }
    public long FailedPings { get; private set; }
    public long TotalTime { get; private set; }
    public decimal AvgTime { get; private set; }
    public decimal RunningAvgTime { get; private set; }
    public long Longest { get; private set; }
    public long Shortest { get; private set; } = long.MaxValue;

    private long runningTotalTime;
    private long runningSuccessfulPings;

    public decimal RecordPing(IPingStats status)
    {
        TotalPings++;

        if (status.Success)
        {
            SuccessfulPings++;
            TotalTime += status.PingTime;
            AvgTime = Math.Round(TotalTime / (decimal)SuccessfulPings, 1);

            runningSuccessfulPings++;
            runningTotalTime += status.PingTime;
            RunningAvgTime = Math.Round(runningTotalTime / (decimal)runningSuccessfulPings, 1);

            if (status.PingTime > Longest)
            {
                Longest = status.PingTime;
            }

            if (status.PingTime < Shortest)
            {
                Shortest = status.PingTime;
            }
        }
        else
        {
            FailedPings++;

            runningTotalTime = 0;
            runningSuccessfulPings = 0;
            RunningAvgTime = 0;
        }

        return Math.Round(SuccessfulPings / (decimal)TotalPings * 100, 1);
    }
}
