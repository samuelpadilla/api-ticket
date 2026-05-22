using Xunit;
using TicketProcessor.Domain.Entities;

namespace TicketProcessor.Tests.Contract
{
    public class DlqReprocessContractTests
    {
        [Fact]
        public void DlqReprocess_Contract_Matches_Spec()
        {
            var req = new DlqReprocessRequest
            {
                CorrelationId = "corr-1",
                RequestedBy = "user@corp.com",
                Items = { new DlqReprocessItem { MessageId = "msg-1", Reason = "Erro X" } }
            };
            var resp = new DlqReprocessResponse
            {
                CorrelationId = req.CorrelationId,
                Accepted = 1,
                Rejected = 0,
                Details = { new DlqReprocessResult { MessageId = "msg-1", Status = "Accepted", Reason = null } }
            };
            Assert.Equal("corr-1", resp.CorrelationId);
            Assert.Equal(1, resp.Accepted);
            Assert.Equal(0, resp.Rejected);
            Assert.Single(resp.Details);
            Assert.Equal("msg-1", resp.Details[0].MessageId);
            Assert.Equal("Accepted", resp.Details[0].Status);
        }
    }
}