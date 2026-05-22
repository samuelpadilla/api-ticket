using System.Collections.Generic;

namespace TicketProcessor.Domain.Entities
{
    public class DlqReprocessRequest
    {
        public string CorrelationId { get; set; } = string.Empty;
        public List<DlqReprocessItem> Items { get; set; } = new();
        public string RequestedBy { get; set; } = string.Empty;
    }

    public class DlqReprocessItem
    {
        public string MessageId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }

    public class DlqReprocessResponse
    {
        public string CorrelationId { get; set; } = string.Empty;
        public int Accepted { get; set; }
        public int Rejected { get; set; }
        public List<DlqReprocessResult> Details { get; set; } = new();
    }

    public class DlqReprocessResult
    {
        public string MessageId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Accepted|Rejected
        public string? Reason { get; set; }
    }
}