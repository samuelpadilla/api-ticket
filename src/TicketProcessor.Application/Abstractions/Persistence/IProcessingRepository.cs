using System.Threading;
using System.Threading.Tasks;

namespace TicketProcessor.Application.Abstractions.Persistence
{
    public interface IProcessingRepository
    {
        Task InsertProcessedMessageAsync(string messageId, string status, CancellationToken cancellationToken = default);
    }
}
