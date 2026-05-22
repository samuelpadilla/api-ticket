using Xunit;

namespace TicketProcessor.Tests
{
    public class HealthCheckTests
    {
        [Fact]
        public void ServiceBusHealthCheck_ShouldReturnTrue()
        {
            // Arrange
            var healthCheck = new TicketProcessor.Worker.Health.ServiceBusHealthCheck();
            // Act
            var result = healthCheck.CheckAsync().Result;
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SqlServerHealthCheck_ShouldReturnTrue()
        {
            var healthCheck = new TicketProcessor.Worker.Health.SqlServerHealthCheck();
            var result = healthCheck.CheckAsync().Result;
            Assert.True(result);
        }
    }
}