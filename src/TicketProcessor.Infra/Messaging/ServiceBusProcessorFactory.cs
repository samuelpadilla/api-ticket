using Azure.Messaging.ServiceBus;

namespace TicketProcessor.Infra.Messaging
{
    public class ServiceBusProcessorFactory
    {
        private readonly string _connectionString;
        public ServiceBusProcessorFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ServiceBusProcessor CreateProcessor(string topicName, string subscriptionName)
        {
            var client = new ServiceBusClient(_connectionString);
            var options = new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 1
            };
            return client.CreateProcessor(topicName, subscriptionName, options);
        }
    }
}
