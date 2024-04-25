﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Ordering.Infastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfastructureServices(this IServiceCollection services, 
                                                                  IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");
            return services;
        }
    }
}
