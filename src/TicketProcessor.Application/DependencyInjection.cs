using Microsoft.Extensions.DependencyInjection;

namespace TicketProcessor.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Adicione serviços da camada Application aqui
            // Exemplo: services.AddTransient<IProcessingService, ProcessingService>();
            return services;
        }
    }
}