using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;

using TicketProcessor.Application.Abstractions.Persistence;

namespace TicketProcessor.Infra.Persistence
{
    public class DapperProcessingRepository : IProcessingRepository
    {
        private readonly SqlConnectionFactory _factory;
        public DapperProcessingRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task InsertProcessedMessageAsync(string messageId, string status, System.Threading.CancellationToken cancellationToken = default)
        {
            using var conn = _factory.CreateConnection();
            await conn.ExecuteAsync(
                "INSERT INTO ProcessedMessages (messageId, firstProcessedAtUtc, lastStatus) VALUES (@messageId, @firstProcessedAtUtc, @lastStatus)",
                new { messageId, firstProcessedAtUtc = DateTime.UtcNow, lastStatus = status });
        }
    }
}
