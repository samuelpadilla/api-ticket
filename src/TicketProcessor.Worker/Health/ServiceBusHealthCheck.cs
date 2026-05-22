using System.Threading;
using System.Threading.Tasks;

namespace TicketProcessor.Worker.Health
{
    public class ServiceBusHealthCheck
    {
        public Task<bool> CheckAsync(CancellationToken cancellationToken = default)
        {
            // Implementação do health check do Service Bus
            return Task.FromResult(true);
        }
    }
}