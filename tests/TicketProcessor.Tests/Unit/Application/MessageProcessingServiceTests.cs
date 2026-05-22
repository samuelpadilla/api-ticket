using Xunit;
using TicketProcessor.Domain.Entities;
using TicketProcessor.Application.Validation;
using TicketProcessor.Domain.Rules;

namespace TicketProcessor.Tests.Unit.Application
{
    public class MessageProcessingServiceTests
    {
        [Fact]
        public void TopicMessageValidator_Should_Validate_Required_Fields()
        {
            var validator = new TopicMessageValidator();
            var msg = new TopicMessage
            {
                MessageId = "id",
                CorrelationId = "corr",
                EventType = "evt",
                Payload = "{}",
                EnqueuedAtUtc = System.DateTime.UtcNow,
                DeliveryCount = 1
            };
            var result = validator.Validate(msg);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void MessageSchemaRule_Should_Validate_Payload()
        {
            var msg = new TopicMessage { Payload = "{\"field\":1}" };
            Assert.True(MessageSchemaRule.IsValidPayload(msg));
        }
    }
}