namespace Pinger.Interfaces;

public interface IRollingStatistics
{
    long Longest { get; }
    long Shortest { get; }
    long TotalTime { get; }
    long TotalPings { get; }
    long FailedPings { get; }
    long SuccessfulPings { get; }
    long StopAfterThisManyPings { get; set; }
    decimal AvgTime { get; }
    decimal RecordPing(IPingStats status);
}
