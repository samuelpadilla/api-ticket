using System;

namespace TicketProcessor.Worker.Observability
{
    public class CorrelationContextAccessor
    {
        public string? CorrelationId { get; set; }
    }
}