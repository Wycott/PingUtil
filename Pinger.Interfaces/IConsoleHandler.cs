namespace Pinger.Interfaces;

public interface IConsoleHandler
{
    void WriteToConsole(string message);
    ConsoleColor ForegroundColour { get; set; }
    void NotifyPingResult(IPingStats status);
}
