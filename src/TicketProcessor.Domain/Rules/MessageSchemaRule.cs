using TicketProcessor.Domain.Entities;

namespace TicketProcessor.Domain.Rules
{
    public static class MessageSchemaRule
    {
        public static bool IsValidPayload(TopicMessage message)
        {
            // Validação de schema mínima por eventType
            // Exemplo: para produção, usar JSON Schema Validator
            return !string.IsNullOrWhiteSpace(message.Payload);
        }
    }
}