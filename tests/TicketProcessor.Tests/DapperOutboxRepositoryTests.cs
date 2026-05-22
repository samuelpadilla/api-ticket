using Xunit;
using TicketProcessor.Infra.Persistence;
using System;

namespace TicketProcessor.Tests
{
    public class DapperOutboxRepositoryTests
    {
        [Fact]
        public void InsertOutboxAsync_ShouldInsertWithoutError()
        {
            // Arrange
            var factory = new SqlConnectionFactory("Server=localhost;Database=TestDb;User Id=sa;Password=Your_password123;");
            var repo = new DapperOutboxRepository(factory);
            // Act & Assert
            var ex = Record.ExceptionAsync(() => repo.InsertOutboxAsync(Guid.NewGuid().ToString(), "{}", "Pending"));
            Assert.Null(ex.Result);
        }
    }
}