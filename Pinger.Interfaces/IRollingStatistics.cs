namespace Pinger.Interfaces
{
    public interface IRollingStatistics
    {
        long Longest { get; set; }
        long Shortest { get; set; }
        long TotalTime { get; set; }
        long TotalPings { get; set; }
        long FailedPings { get; set; }
        long SuccessfulPings { get; set; }
        long StopAfterThisManyPings { get; set; }
        decimal AvgTime { get; set; }
    }
}
