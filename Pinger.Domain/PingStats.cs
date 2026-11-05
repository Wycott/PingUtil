using Pinger.Interfaces;

namespace Pinger.Domain;

public class PingStats : IPingStats
{
    public bool Success { get; init; }
    public long PingTime { get; init; }
}
