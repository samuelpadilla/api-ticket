using System.Threading;
using System.Threading.Tasks;
using TicketProcessor.Application.UseCases.ReprocessDlq;
using TicketProcessor.Domain.Entities;

namespace TicketProcessor.Worker.Operational
{
    public class DlqReprocessController
    {
        private readonly ReprocessDlqHandler _handler;
        public DlqReprocessController(ReprocessDlqHandler handler)
        {
            _handler = handler;
        }

        public async Task<DlqReprocessResponse> ReprocessAsync(DlqReprocessRequest request, CancellationToken cancellationToken = default)
        {
            // Aqui seria exposto via endpoint operacional (ex: HTTP)
            return await _handler.HandleAsync(request, cancellationToken);
        }
    }
}