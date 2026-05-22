using System.Threading;
using System.Threading.Tasks;

namespace TicketProcessor.Worker.Health
{
    public class SqlServerHealthCheck
    {
        public Task<bool> CheckAsync(CancellationToken cancellationToken = default)
        {
            // Implementação do health check do SQL Server
            return Task.FromResult(true);
        }
    }
}