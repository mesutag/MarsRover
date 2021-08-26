using MarsRover.Application.Behaivor;
using MarsRover.Inftrastructure;
using MarsRover.Inftrastructure.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MarsRover.Application.Commands.CreatePlateau;
using MarsRover.Application.Commands.DeployRover;
using MarsRover.Core.AggregateRoots.PlateauAggregate;

namespace MarsRover.Application.Registry
{
    public static class ApplicationRegistryExtension
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CreatePlateauCommandHandler));
            services.AddDbContext<MarsRoverContext>(options => options.UseInMemoryDatabase(databaseName: "MarsRoverDatabase"));
            services.AddRepositories();
            services.AddValidations();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            return services;

        }
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPlateauRepository, PlateauRepository>();
            return services;
        }
        private static IServiceCollection AddValidations(this IServiceCollection services)
        {
            services.AddTransient<IValidator<CreatePlateauCommand>, CreatePlateauCommandValidator>();
            services.AddTransient<IValidator<DeployRoverCommand>, DeployRoverCommandValidator>();
            return services;

        }
    }
}
