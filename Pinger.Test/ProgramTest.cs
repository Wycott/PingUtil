using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Pinger.Interfaces;

namespace Pinger.Test;

public class ProgramTest
{
    [Fact]
    public void StartWork_CallsEngineStart()
    {
        var engine = new Mock<IPingEngine>();

        Program.StartWork(engine.Object);

        engine.Verify(x => x.Start(), Times.Once);
    }

    [Fact]
    public void ConfigureServices_ResolvesIPingEngine()
    {
        var serviceProvider = Program.ConfigureServices();

        var engine = serviceProvider.GetService<IPingEngine>();

        engine.Should().NotBeNull();
    }
}
