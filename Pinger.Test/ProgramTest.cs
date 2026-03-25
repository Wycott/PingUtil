using AiAnnotations;
using AiAnnotations.Types;
using FluentAssertions;
using Moq;
using Pinger.Interfaces;

namespace Pinger.Test;

[AiGenerated(Authorship.Hybrid)]
public class ProgramTest
{
    [Fact]
    public void WhenInvoked_ThenSuccess()
    {
        // Arrange
        var engine = new Mock<IPingEngine>();

        // Act/Assert
        Program.StartWork(engine.Object);
    }

    [Fact]
    public void StartWork_CallsEngineStart()
    {
        var engine = new Mock<IPingEngine>();

        Program.StartWork(engine.Object);

        engine.Verify(x => x.Start(), Times.Once);
    }
}
