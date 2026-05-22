namespace TicketProcessor.Domain.Entities
{
    public class DeadLetterItem
    {
        public string MessageId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public int Attempt { get; set; }
    }
}