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
    public long Longest { get; private set; }
    public long Shortest { get; private set; } = long.MaxValue;

    public decimal RecordPing(IPingStats status)
    {
        TotalPings++;

        if (status.Success)
        {
            SuccessfulPings++;
            TotalTime += status.PingTime;
            AvgTime = Math.Round(TotalTime / (decimal)SuccessfulPings, 1);

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
        }

        return Math.Round(SuccessfulPings / (decimal)TotalPings * 100, 1);
    }
}
