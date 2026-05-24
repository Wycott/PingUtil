using Microsoft.Extensions.Configuration;
using Pinger.Interfaces;

namespace Pinger.Domain;

public class PingConfig : IPingConfig
{
    public string Data { get; set; } = "abcdefghijklmnopqrstuvwxyz012345";
    public string RemoteServer { get; set; } = "8.8.8.8";
    public int SnoozeTime { get; set; } = 5000;
    public int Timeout { get; set; } = 10000;
    public bool PingerIsActive { get; set; } = true;
    public int WorkingHours { get; set; } = 16;
    public int AlertAfterThisManyFailedPings { get; set; } = 5;
    public string CodeName { get; set; } = "Oh Well";

    public PingConfig()
    {
    }

    public PingConfig(IConfiguration configuration)
    {
        var section = configuration.GetSection("PingSettings");
        if (section.Exists())
        {
            section.Bind(this);
        }

        if (string.IsNullOrEmpty(Data))
        {
            Data = "abcdefghijklmnopqrstuvwxyz012345";
        }
    }
}
