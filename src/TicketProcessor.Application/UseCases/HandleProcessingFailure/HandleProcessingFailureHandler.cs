using System.Threading;
using System.Threading.Tasks;
using TicketProcessor.Domain.Entities;
using TicketProcessor.Domain.Enums;
using TicketProcessor.Application.Abstractions.Messaging;

namespace TicketProcessor.Application.UseCases.HandleProcessingFailure
{
    public class HandleProcessingFailureHandler
    {
        private readonly IDeadLetterPublisher _dlqPublisher;
        public HandleProcessingFailureHandler(IDeadLetterPublisher dlqPublisher)
        {
            _dlqPublisher = dlqPublisher;
        }

        public async Task<ProcessingStatus> HandleAsync(DeadLetterItem item, bool isTransient, int maxAttempts, CancellationToken cancellationToken = default)
        {
            if (isTransient && item.Attempt < maxAttempts)
            {
                // Retry
                return ProcessingStatus.Failed;
            }
            else
            {
                // Dead-letter
                await _dlqPublisher.PublishToDeadLetterAsync(item.MessageId, item.Reason, cancellationToken);
                return ProcessingStatus.DeadLettered;
            }
        }
    }
}