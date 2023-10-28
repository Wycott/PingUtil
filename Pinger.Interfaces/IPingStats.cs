namespace Pinger.Interfaces;

public interface IPingStats
{
    bool Success { get; set; }
    long PingTime { get; set; }
}