using OpenTelemetry.Trace;
using Microsoft.Extensions.DependencyInjection;

namespace TicketProcessor.Worker.Observability
{
    public static class TelemetrySetup
    {
        public static IServiceCollection AddTelemetry(this IServiceCollection services)
        {
            services.AddOpenTelemetryTracing(builder =>
            {
                builder.AddAspNetCoreInstrumentation();
                builder.AddHttpClientInstrumentation();
                builder.AddConsoleExporter();
            });
            return services;
        }
    }
}