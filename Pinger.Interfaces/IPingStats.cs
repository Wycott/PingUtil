namespace Pinger.Interfaces;

public interface IPingStats
{
    bool Success { get; }
    long PingTime { get; }
}
