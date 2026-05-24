namespace Pinger.Interfaces;

public interface IConsoleHandler
{
    void WriteToConsole(string message);
    void Beep();
    ConsoleColor ForegroundColour { get; set; }
    void NotifyPingResult(IPingStats status);
}
