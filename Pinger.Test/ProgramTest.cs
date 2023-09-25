using Moq;
using Pinger.Interfaces;

namespace Pinger.Test
{
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
    }
}
