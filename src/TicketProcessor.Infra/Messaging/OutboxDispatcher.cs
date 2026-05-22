using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace TicketProcessor.Infra.Messaging
{
    public class OutboxDispatcher
    {
        public Task DispatchAsync(string messageId, string payload)
        {
            // Implementação do envio para o tópico ou fila
            // Exemplo: publicar no Service Bus
            return Task.CompletedTask;
        }
    }
}
