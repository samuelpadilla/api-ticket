using System.Threading;
using System.Threading.Tasks;
using TicketProcessor.Domain.Entities;
using TicketProcessor.Application.Validation;
using TicketProcessor.Domain.Rules;
using TicketProcessor.Application.Abstractions.Persistence;

namespace TicketProcessor.Application.UseCases.ProcessTopicMessage
{
    public class ProcessTopicMessageHandler
    {
        private readonly IProcessingRepository _repository;
        private readonly TopicMessageValidator _validator;
        public ProcessTopicMessageHandler(IProcessingRepository repository)
        {
            _repository = repository;
            _validator = new TopicMessageValidator();
        }

        public async Task<bool> HandleAsync(TopicMessage message, CancellationToken cancellationToken = default)
        {
            var validation = _validator.Validate(message);
            if (!validation.IsValid || !MessageSchemaRule.IsValidPayload(message))
                return false;

            await _repository.InsertProcessedMessageAsync(message.MessageId, "Persisted", cancellationToken);
            return true;
        }
    }
}