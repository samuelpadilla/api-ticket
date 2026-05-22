using Xunit;
using TicketProcessor.Application.UseCases.HandleProcessingFailure;
using TicketProcessor.Domain.Entities;
using TicketProcessor.Domain.Enums;
using System.Threading.Tasks;

namespace TicketProcessor.Tests.Unit.Infra
{
    public class RetryAndFailureClassificationTests
    {
        [Fact]
        public async Task Handler_Should_DeadLetter_When_MaxAttempts_Exceeded()
        {
            var dlq = new TestDlqPublisher();
            var handler = new HandleProcessingFailureHandler(dlq);
            var item = new DeadLetterItem { MessageId = "msg-1", Reason = "fail", Attempt = 3 };
            var status = await handler.HandleAsync(item, isTransient: true, maxAttempts: 3);
            Assert.Equal(ProcessingStatus.DeadLettered, status);
            Assert.True(dlq.Published);
        }

        private class TestDlqPublisher : TicketProcessor.Application.Abstractions.Messaging.IDeadLetterPublisher
        {
            public bool Published { get; private set; }
            public Task PublishToDeadLetterAsync(string messageId, string reason, System.Threading.CancellationToken cancellationToken = default)
            {
                Published = true;
                return Task.CompletedTask;
            }
        }
    }
}