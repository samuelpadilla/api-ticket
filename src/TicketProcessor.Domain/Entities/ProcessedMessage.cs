using System;

namespace TicketProcessor.Domain.Entities
{
    public class ProcessedMessage
    {
        public string MessageId { get; set; } = string.Empty;
        public DateTime FirstProcessedAtUtc { get; set; }
        public string LastStatus { get; set; } = string.Empty;
    }
}