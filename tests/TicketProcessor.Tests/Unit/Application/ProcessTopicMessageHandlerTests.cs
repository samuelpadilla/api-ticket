using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TicketProcessor.Application.UseCases.ProcessTopicMessage;
using TicketProcessor.Application.Abstractions.Persistence;
using TicketProcessor.Domain.Entities;

namespace TicketProcessor.Tests.Unit.Application
{
    public class ProcessTopicMessageHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldCallRepository()
        {
            // Arrange
            var mockRepo = new Mock<IProcessingRepository>();
            mockRepo
                .Setup(repo => repo.InsertProcessedMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new ProcessTopicMessageHandler(mockRepo.Object);
            var message = new TopicMessage
            {
                MessageId = "msg-1",
                CorrelationId = "corr-1",
                EventType = "TestEvent",
                Payload = "{}",
                EnqueuedAtUtc = System.DateTime.UtcNow,
                DeliveryCount = 1
            };

            // Act
            await handler.HandleAsync(message, CancellationToken.None);

            // Assert
            mockRepo.Verify(repo => repo.InsertProcessedMessageAsync("msg-1", "Processed", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}