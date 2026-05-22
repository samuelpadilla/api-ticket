using Xunit;
using TicketProcessor.Infra.Messaging;

namespace TicketProcessor.Tests
{
    public class ServiceBusProcessorFactoryTests
    {
        [Fact]
        public void CreateProcessor_ShouldReturnProcessor()
        {
            var factory = new ServiceBusProcessorFactory("Endpoint=sb://test/;SharedAccessKeyName=Root;SharedAccessKey=key;");
            var processor = factory.CreateProcessor("topic", "subscription");
            Assert.NotNull(processor);
        }
    }
}