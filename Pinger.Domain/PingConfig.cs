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
    public string CodeName { get; set; } = "Oh Well"; // Overridden by appsettings.json

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

        Validate();
    }

    private void Validate()
    {
        // Ping.Send rejects larger buffers
        const int maxDataLength = 65500;

        var problems = new List<string>();

        if (string.IsNullOrWhiteSpace(RemoteServer))
        {
            problems.Add("RemoteServer must be set");
        }

        if (Timeout <= 0)
        {
            problems.Add("Timeout must be greater than zero");
        }

        if (SnoozeTime <= 0)
        {
            problems.Add("SnoozeTime must be greater than zero");
        }

        if (WorkingHours <= 0)
        {
            problems.Add("WorkingHours must be greater than zero");
        }

        if (AlertAfterThisManyFailedPings <= 0)
        {
            problems.Add("AlertAfterThisManyFailedPings must be greater than zero");
        }

        if (Data.Length > maxDataLength)
        {
            problems.Add($"Data must be at most {maxDataLength} characters");
        }

        if (problems.Count > 0)
        {
            throw new InvalidOperationException($"Invalid PingSettings in appsettings.json: {string.Join("; ", problems)}.");
        }
    }
}
