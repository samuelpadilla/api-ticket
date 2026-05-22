using Xunit;
using TicketProcessor.Infra.Persistence;
using System;

namespace TicketProcessor.Tests
{
    public class DapperProcessingRepositoryTests
    {
        [Fact]
        public void InsertProcessedMessageAsync_ShouldInsertWithoutError()
        {
            // Arrange
            var factory = new SqlConnectionFactory("Server=localhost;Database=TestDb;User Id=sa;Password=Your_password123;");
            var repo = new DapperProcessingRepository(factory);
            // Act & Assert
            var ex = Record.ExceptionAsync(() => repo.InsertProcessedMessageAsync(Guid.NewGuid().ToString(), "Persisted"));
            Assert.Null(ex.Result);
        }
    }
}