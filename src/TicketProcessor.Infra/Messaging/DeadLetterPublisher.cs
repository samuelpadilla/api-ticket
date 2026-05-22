using System.Threading;
using System.Threading.Tasks;
using TicketProcessor.Application.Abstractions.Messaging;

namespace TicketProcessor.Infra.Messaging
{
    public class DeadLetterPublisher : IDeadLetterPublisher
    {
        public Task PublishToDeadLetterAsync(string messageId, string reason, CancellationToken cancellationToken = default)
        {
            // Simulação de envio para DLQ
            return Task.CompletedTask;
        }
    }
}