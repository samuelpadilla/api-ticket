namespace TicketProcessor.Application.Configuration
{
    public class ServiceBusOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string TopicName { get; set; } = string.Empty;
        public string SubscriptionName { get; set; } = string.Empty;
    }
}