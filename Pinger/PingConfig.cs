using Pinger.Interfaces;

namespace Pinger;

public class PingConfig : IPingConfig
{
    public string Data { get; set; } = "All our lives we sweat and save.";
    public string RemoteServer { get; set; } = "8.8.8.8";
    public int SnoozeTime { get; set; } = 5000;
    public int Timeout { get; set; } = 10000;
    public bool PingerIsActive { get; set; } = true;
}