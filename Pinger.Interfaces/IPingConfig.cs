namespace Pinger.Interfaces;

public interface IPingConfig
{
    int Timeout { get; set; }
    int SnoozeTime { get; set; }
    int WorkingHours { get; set; }
    int AlertAfterThisManyFailedPings { get; set; }
    bool PingerIsActive { get; set; }
    string Data { get; set; }
    string CodeName { get; set; }
    string RemoteServer { get; set; }
}