namespace Pinger.Interfaces;

public interface IRollingStatistics
{
    long Longest { get; }
    long Shortest { get; }
    // ReSharper disable once UnusedMemberInSuper.Global
    long TotalTime { get; }
    long TotalPings { get; }
    long FailedPings { get; }
    long SuccessfulPings { get; }
    long StopAfterThisManyPings { get; set; }
    decimal AvgTime { get; }
    decimal RecordPing(IPingStats status);
}
