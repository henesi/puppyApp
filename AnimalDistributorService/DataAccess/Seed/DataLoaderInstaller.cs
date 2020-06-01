using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDistributorService.DataAccess.Seed
{
    public static class DataLoaderInstaller
    {
        public static IServiceCollection AddAnimalDemoInitializer(this IServiceCollection services)
        {
            services.AddScoped<DataLoader>();
            return services;
        }
    }
}
