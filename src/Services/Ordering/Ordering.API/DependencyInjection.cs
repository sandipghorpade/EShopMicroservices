using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddCarter();
            services.AddExceptionHandler<CustomExceptionHandler>();
            services.AddHealthChecks()
                    .AddSqlServer(config.GetConnectionString("Database")!);
            return services;
        }

        public static WebApplication UseApiServices(this WebApplication webApplication)
        {
            webApplication.MapCarter();
            webApplication.UseExceptionHandler(options => { });
            webApplication.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            return webApplication;
        }
    }
}
