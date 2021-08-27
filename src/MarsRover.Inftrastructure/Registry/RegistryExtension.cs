using MarsRover.Inftrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MarsRover.Inftrastructure.DatabaseContext;
using System;

namespace MarsRover.Inftrastructure.Registry
{
    public static class InfrastructureRegistryExtension
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, Action<MarsRoverContextOption> option)
        {
            services.AddDbContext<MarsRoverContext>();
            services.Configure(option);
            services.AddRepositories();

            return services;

        }
        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPlateauRepository, PlateauRepository>();
        }
    }
}
