using Microsoft.Extensions.DependencyInjection;
using TicketProcessor.Infra.Messaging;
using TicketProcessor.Infra.Persistence;

namespace TicketProcessor.Infra
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton(new SqlConnectionFactory(connectionString));
            services.AddTransient<DapperProcessingRepository>();
            services.AddTransient<DapperOutboxRepository>();
            services.AddTransient<ServiceBusProcessorFactory>();
            services.AddTransient<ServiceBusTopicConsumer>();
            services.AddTransient<OutboxDispatcher>();
            services.AddTransient<TicketProcessor.Application.Abstractions.Messaging.IDeadLetterPublisher, DeadLetterPublisher>();
            services.AddTransient<TicketProcessor.Application.Abstractions.Messaging.IOutboxDispatcher, OutboxDispatcher>();
            services.AddTransient<TicketProcessor.Application.Abstractions.Persistence.IProcessingRepository, DapperProcessingRepository>();
            return services;
        }
    }
}