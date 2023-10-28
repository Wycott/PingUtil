namespace Pinger.Interfaces;

public interface IPingConfig
{
    string Data { get; set; }
    string RemoteServer { get; set; }
    int SnoozeTime { get; set; }
    int Timeout { get; set; }
    bool PingerIsActive { get; set; }
    int WorkingHours { get; set; }
}