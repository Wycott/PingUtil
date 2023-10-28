namespace Pinger.Interfaces;

public interface IConsoleHandler
{
    void WriteToConsole(string message);
    ConsoleColor ForegroundColour { get; set; }
    long AudioCue(IPingStats status, long failedPingsInCluster);
}