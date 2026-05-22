using System.Threading;
using System.Threading.Tasks;

namespace TicketProcessor.Worker.Workers
{
    using Microsoft.Extensions.Logging;
    using TicketProcessor.Application.UseCases.ProcessTopicMessage;
    using TicketProcessor.Domain.Entities;
    using TicketProcessor.Worker.Observability;

    public class TopicConsumerWorker
    {
        private readonly ProcessTopicMessageHandler _handler;
        private readonly ILogger<TopicConsumerWorker> _logger;

        public TopicConsumerWorker(ProcessTopicMessageHandler handler, ILogger<TopicConsumerWorker> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            // Simulação de consumo de mensagens
            while (!cancellationToken.IsCancellationRequested)
            {
                var message = new TopicMessage
                {
                    MessageId = Guid.NewGuid().ToString(),
                    CorrelationId = Guid.NewGuid().ToString(),
                    EventType = "TestEvent",
                    Payload = "{ }",
                    EnqueuedAtUtc = DateTime.UtcNow,
                    DeliveryCount = 1
                };

                var retryPolicy = TicketProcessor.Infra.Resilience.RetryPolicies.GetDefaultRetryPolicy();
                int attempt = 0;
                bool success = false;
                Exception? lastException = null;
                while (attempt < 3 && !success)
                {
                    attempt++;
                    try
                    {
                        await retryPolicy.ExecuteAsync(async () =>
                        {
                            await _handler.HandleAsync(message, cancellationToken);
                        });
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                    }
                }
                if (success)
                {
                    ProcessingMetrics.ProcessedMessagesCount++;
                    _logger.LogInformation("Mensagem processada com sucesso: {MessageId}", message.MessageId);
                }
                else
                {
                    var dlqPublisher = new TicketProcessor.Infra.Messaging.DeadLetterPublisher();
                    await dlqPublisher.PublishToDeadLetterAsync(message.MessageId, lastException?.Message ?? "Erro desconhecido", cancellationToken);
                    _logger.LogWarning("Mensagem enviada para DLQ: {MessageId}", message.MessageId);
                }

                await Task.Delay(1000, cancellationToken);
            }
            }
    }
}
}
