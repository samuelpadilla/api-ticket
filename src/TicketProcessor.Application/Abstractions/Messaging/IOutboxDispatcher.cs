using System.Threading;
using System.Threading.Tasks;

namespace TicketProcessor.Application.Abstractions.Messaging
{
    public interface IOutboxDispatcher
    {
        Task DispatchAsync(string messageId, string payload, CancellationToken cancellationToken = default);
    }
}