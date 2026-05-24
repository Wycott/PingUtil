namespace Pinger.Interfaces;

public interface IPingConfig
{
    int Timeout { get; }
    int SnoozeTime { get; }
    int WorkingHours { get; }
    int AlertAfterThisManyFailedPings { get; }
    bool PingerIsActive { get; }
    string Data { get; }
    string CodeName { get; }
    string RemoteServer { get; }
}
