using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace TicketProcessor.Infra.Persistence
{
    public class DapperOutboxRepository
    {
        private readonly SqlConnectionFactory _factory;
        public DapperOutboxRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task InsertOutboxAsync(string messageId, string payload, string status)
        {
            using var conn = _factory.CreateConnection();
            await conn.ExecuteAsync("INSERT INTO Outbox (messageId, payload, status) VALUES (@messageId, @payload, @status)",
                new { messageId, payload, status });
        }
    }
}
