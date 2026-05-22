using Microsoft.Extensions.DependencyInjection;

namespace TicketProcessor.Worker
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWorkerServices(this IServiceCollection services)
        {
            // Adicione serviços da camada Worker aqui
            // Exemplo: services.AddHostedService<TopicConsumerWorker>();
            return services;
        }
    }
}