using Xunit;
using TicketProcessor.Worker.Workers;
using System.Threading.Tasks;

namespace TicketProcessor.Tests
{
    public class TopicConsumerWorkerTests
    {
        [Fact]
        public async Task ExecuteAsync_ShouldCompleteWithoutError()
        {
            var worker = new TopicConsumerWorker();
            var ex = await Record.ExceptionAsync(() => worker.ExecuteAsync());
            Assert.Null(ex);
        }
    }
}