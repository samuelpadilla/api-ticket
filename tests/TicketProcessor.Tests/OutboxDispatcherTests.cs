using Xunit;
using TicketProcessor.Infra.Messaging;
using System.Threading.Tasks;

namespace TicketProcessor.Tests
{
    public class OutboxDispatcherTests
    {
        [Fact]
        public async Task DispatchAsync_ShouldCompleteWithoutError()
        {
            var dispatcher = new OutboxDispatcher();
            var ex = await Record.ExceptionAsync(() => dispatcher.DispatchAsync("msg-1", "{}"));
            Assert.Null(ex);
        }
    }
}