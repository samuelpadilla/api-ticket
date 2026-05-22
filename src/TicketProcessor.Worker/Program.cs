using Microsoft.Extensions.DependencyInjection;
using TicketProcessor.Worker.Health;

namespace TicketProcessor.Worker
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddSingleton<ServiceBusHealthCheck>();
            services.AddSingleton<SqlServerHealthCheck>();
            // Adicione inicialização do host, DI, observabilidade, etc.
        }
    }
}