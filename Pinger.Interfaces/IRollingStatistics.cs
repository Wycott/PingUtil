namespace Pinger.Interfaces
{
    public interface IRollingStatistics
    {
        long StopAfterThisManyPings { get; set; }
        long TotalPings { get; set; }
        long SuccessfulPings { get; set; }
        long FailedPings { get; set; }
        long TotalTime { get; set; }
        decimal AvgTime { get; set; }
        long Longest { get; set; }
        long Shortest { get; set; }
    }
}
