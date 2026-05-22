using System;

namespace TicketProcessor.Domain.Entities
{
    public class ProcessingRecord
    {
        public Guid ProcessingId { get; set; }
        public string MessageId { get; set; } = string.Empty;
        public string CorrelationId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Attempt { get; set; }
        public string? FailureReason { get; set; }
        public DateTime ProcessedAtUtc { get; set; }
    }
}