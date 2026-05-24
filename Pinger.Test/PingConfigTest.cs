using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Pinger.Domain;

namespace Pinger.Test;

public class PingConfigTest
{
    [Fact]
    public void DefaultValues_AreCorrect()
    {
        var config = new PingConfig();

        config.RemoteServer.Should().Be("8.8.8.8");
        config.Timeout.Should().Be(10000);
        config.SnoozeTime.Should().Be(5000);
        config.WorkingHours.Should().Be(16);
        config.AlertAfterThisManyFailedPings.Should().Be(5);
        config.PingerIsActive.Should().BeTrue();
        config.Data.Should().Be("abcdefghijklmnopqrstuvwxyz012345");
        config.CodeName.Should().Be("Oh Well");
    }

    [Fact]
    public void Properties_AreSettable()
    {
        var config = new PingConfig
        {
            Data = "custom data",
            RemoteServer = "127.0.0.1",
            PingerIsActive = false,
            SnoozeTime = 3000,
            Timeout = 5000,
            WorkingHours = 4,
            AlertAfterThisManyFailedPings = 10,
            CodeName = "Test Name"
        };

        config.Data.Should().Be("custom data");
        config.RemoteServer.Should().Be("127.0.0.1");
        config.SnoozeTime.Should().Be(3000);
        config.Timeout.Should().Be(5000);
        config.WorkingHours.Should().Be(4);
        config.PingerIsActive.Should().BeFalse();
        config.AlertAfterThisManyFailedPings.Should().Be(10);
        config.CodeName.Should().Be("Test Name");
    }

    [Fact]
    public void Constructor_WithConfiguration_BindsValues()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "PingSettings:RemoteServer", "1.1.1.1" },
            { "PingSettings:Timeout", "2000" },
            { "PingSettings:SnoozeTime", "1000" },
            { "PingSettings:WorkingHours", "8" },
            { "PingSettings:PingerIsActive", "false" },
            { "PingSettings:CodeName", "Loaded" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var config = new PingConfig(configuration);

        config.RemoteServer.Should().Be("1.1.1.1");
        config.Timeout.Should().Be(2000);
        config.SnoozeTime.Should().Be(1000);
        config.WorkingHours.Should().Be(8);
        config.PingerIsActive.Should().BeFalse();
        config.CodeName.Should().Be("Loaded");
    }

    [Fact]
    public void Constructor_WithEmptyConfiguration_UsesDefaults()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        var config = new PingConfig(configuration);

        config.RemoteServer.Should().Be("8.8.8.8");
        config.Timeout.Should().Be(10000);
        config.SnoozeTime.Should().Be(5000);
    }
}
