namespace TicketProcessor.Application.Configuration
{
    public class RetryOptions
    {
        public int MaxAttempts { get; set; } = 3;
        public int InitialDelaySeconds { get; set; } = 2;
        public int MaxDelaySeconds { get; set; } = 30;
    }
}