using System.Threading;
using System.Threading.Tasks;

namespace TicketProcessor.Application.Abstractions.Messaging
{
    public interface IDeadLetterPublisher
    {
        Task PublishToDeadLetterAsync(string messageId, string reason, CancellationToken cancellationToken = default);
    }
}