using Azure.Messaging.ServiceBus;

namespace TicketProcessor.Infra.Messaging
{
    public class ServiceBusTopicConsumer
    {
        private readonly ServiceBusProcessor _processor;

        public ServiceBusTopicConsumer(ServiceBusProcessor processor)
        {
            _processor = processor;
        }

        public void RegisterHandlers(Func<ProcessMessageEventArgs, Task> messageHandler, Func<ProcessErrorEventArgs, Task> errorHandler)
        {
            _processor.ProcessMessageAsync += messageHandler;
            _processor.ProcessErrorAsync += errorHandler;
        }

        public async Task StartProcessingAsync(CancellationToken cancellationToken = default)
        {
            await _processor.StartProcessingAsync(cancellationToken);
        }

        public async Task StopProcessingAsync(CancellationToken cancellationToken = default)
        {
            await _processor.StopProcessingAsync(cancellationToken);
        }
    }
}
