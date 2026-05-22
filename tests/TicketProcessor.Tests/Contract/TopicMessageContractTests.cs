using Xunit;
using TicketProcessor.Domain.Entities;

namespace TicketProcessor.Tests.Contract
{
    public class TopicMessageContractTests
    {
        [Fact]
        public void TopicMessage_Contract_Matches_Spec()
        {
            var msg = new TopicMessage
            {
                MessageId = "abc123",
                CorrelationId = "corr-1",
                EventType = "event-type",
                Payload = "{\"field\":123}",
                EnqueuedAtUtc = System.DateTime.UtcNow,
                DeliveryCount = 1
            };

            Assert.False(string.IsNullOrWhiteSpace(msg.MessageId));
            Assert.False(string.IsNullOrWhiteSpace(msg.CorrelationId));
            Assert.False(string.IsNullOrWhiteSpace(msg.EventType));
            Assert.False(string.IsNullOrWhiteSpace(msg.Payload));
            Assert.True(msg.DeliveryCount >= 1);
        }
    }
}