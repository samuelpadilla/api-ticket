using System;

namespace TicketProcessor.Domain.Entities
{
    public class TopicMessage
    {
        public string MessageId { get; set; } = string.Empty;
        public string CorrelationId { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public DateTime EnqueuedAtUtc { get; set; }
        public int DeliveryCount { get; set; }
    }
}