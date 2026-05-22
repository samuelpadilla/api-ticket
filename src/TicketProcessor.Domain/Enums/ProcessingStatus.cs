namespace TicketProcessor.Domain.Enums
{
    public enum ProcessingStatus
    {
        Received,
        Validated,
        Persisted,
        Failed,
        DeadLettered,
        Reprocessed
    }
}