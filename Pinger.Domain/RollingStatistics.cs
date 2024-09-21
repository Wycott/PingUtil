using Pinger.Interfaces;

namespace Pinger.Domain
{
    public class RollingStatistics : IRollingStatistics
    {
        public long StopAfterThisManyPings { get; set; }
        public long TotalPings { get; set; }
        public long SuccessfulPings { get; set; }
        public long FailedPings { get; set; }
        public long TotalTime { get; set; }
        public decimal AvgTime { get; set; }
        public long Longest { get; set; }
        public long Shortest { get; set; }
    }
}
