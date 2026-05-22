using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TicketProcessor.Domain.Entities;

namespace TicketProcessor.Application.UseCases.ReprocessDlq
{
    public class ReprocessDlqHandler
    {
        public async Task<DlqReprocessResponse> HandleAsync(DlqReprocessRequest request, CancellationToken cancellationToken = default)
        {
            // Simulação de reprocessamento
            var response = new DlqReprocessResponse
            {
                CorrelationId = request.CorrelationId,
                Accepted = request.Items.Count,
                Rejected = 0,
                Details = new List<DlqReprocessResult>()
            };
            foreach (var item in request.Items)
            {
                response.Details.Add(new DlqReprocessResult
                {
                    MessageId = item.MessageId,
                    Status = "Accepted",
                    Reason = null
                });
            }
            await Task.CompletedTask;
            return response;
        }
    }
}