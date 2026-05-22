using System.Threading;
using System.Threading.Tasks;

namespace TicketProcessor.Application.Abstractions.Messaging
{
    public interface IMessageConsumer
    {
        Task ConsumeAsync(CancellationToken cancellationToken = default);
    }
}
