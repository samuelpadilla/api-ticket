using Xunit;
using TicketProcessor.Application.UseCases.ReprocessDlq;
using TicketProcessor.Domain.Entities;
using System.Threading.Tasks;

namespace TicketProcessor.Tests.Unit.Application
{
    public class ReprocessDlqHandlerTests
    {
        [Fact]
        public async Task Handler_Should_Accept_All_Items()
        {
            var handler = new ReprocessDlqHandler();
            var req = new DlqReprocessRequest
            {
                CorrelationId = "corr-1",
                RequestedBy = "user@corp.com",
                Items = { new DlqReprocessItem { MessageId = "msg-1", Reason = "Erro X" } }
            };
            var resp = await handler.HandleAsync(req);
            Assert.Equal(1, resp.Accepted);
            Assert.Equal(0, resp.Rejected);
            Assert.Single(resp.Details);
            Assert.Equal("msg-1", resp.Details[0].MessageId);
            Assert.Equal("Accepted", resp.Details[0].Status);
        }
    }
}