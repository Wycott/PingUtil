using Pinger.Interfaces;

namespace Pinger;

public class PingStats : IPingStats
{
    public bool Success { get; set; }
    public long PingTime { get; set; }
}

