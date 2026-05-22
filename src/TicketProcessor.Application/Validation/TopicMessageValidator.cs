using FluentValidation;
using TicketProcessor.Domain.Entities;

namespace TicketProcessor.Application.Validation
{
    public class TopicMessageValidator : AbstractValidator<TopicMessage>
    {
        public TopicMessageValidator()
        {
            RuleFor(x => x.MessageId).NotEmpty();
            RuleFor(x => x.CorrelationId).NotEmpty();
            RuleFor(x => x.EventType).NotEmpty();
            RuleFor(x => x.Payload).NotEmpty();
            RuleFor(x => x.EnqueuedAtUtc).NotEqual(default(DateTime));
            RuleFor(x => x.DeliveryCount).GreaterThanOrEqualTo(1);
        }
    }
}