using MarsRover.Application.Behaivor;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MarsRover.Application.Commands.CreatePlateau;
using MarsRover.Application.Commands.DeployRover;

namespace MarsRover.Application.Registry
{
    public static class RegistryExtension
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CreatePlateauCommandHandler));
            services.AddValidations();
            services.AddCommandHelpers();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            return services;

        }
        private static IServiceCollection AddValidations(this IServiceCollection services)
        {
            services.AddTransient<IValidator<CreatePlateauCommand>, CreatePlateauCommandValidator>();
            services.AddTransient<IValidator<DeployRoverCommand>, DeployRoverCommandValidator>();
            return services;

        }
        private static IServiceCollection AddCommandHelpers(this IServiceCollection services)
        {
            services.AddScoped<ICreatePlateauCommandHelper, CreatePlateauCommandHelper>();
            services.AddScoped<IDeployRoverCommandHelper, DeployRoverCommandHelper>();
            return services;

        }
    }
}
