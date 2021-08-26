using MarsRover.Application.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using MarsRover.Application.Registry;
using MarsRover.Application.Commands.DeployRover;
using MarsRover.Application.Commands.CreatePlateau;
using MarsRover.Application.Commands.StartRover;

namespace MarsRover.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = Initialize();
            var _mediator = serviceProvider.GetRequiredService<IMediator>();

            do
            {
                try
                {
                    Console.WriteLine("Enter Plateau Size:");
                    var plateauSize = Console.ReadLine();

                    var createPlateauCommand = new CreatePlateauCommand(plateauSize);
                    var createPlateauResponse = await _mediator.Send(createPlateauCommand);

                    do
                    {
                        Guid roverId = Guid.Empty;
                        try
                        {
                            Console.WriteLine("Enter Rover Position:");
                            var roverPosition = Console.ReadLine();


                            Console.WriteLine("Enter Rover Directions:");
                            var roverDirections = Console.ReadLine().Trim();

                            var deployRoverCommand = new DeployRoverCommand(createPlateauResponse.Id, roverPosition, roverDirections);
                            var deployRoverResponse = await _mediator.Send(deployRoverCommand);

                            Console.WriteLine("Do you want to deploy another rover this plateau? (Y/N)");
                            var result = Console.ReadLine().Trim();
                            if (result.Equals("N", StringComparison.InvariantCultureIgnoreCase))
                            {
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("---------------");
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Do you want to try again? (Y/N)");
                            var result = Console.ReadLine();
                            if (result.Equals("N", StringComparison.InvariantCultureIgnoreCase))
                            {
                                break;
                            }
                        }
                    } while (true);

                    var plateauGetResponse = await _mediator.Send(new GetPlateauByIdQuery(createPlateauResponse.Id));

                    foreach (var rover in plateauGetResponse.Rovers)
                    {
                        await _mediator.Send(new StartRoverMissionCommand(plateauGetResponse.PlateauId, rover.RoverId));
                    }

                    plateauGetResponse = await _mediator.Send(new GetPlateauByIdQuery(createPlateauResponse.Id));
                    foreach (var rover in plateauGetResponse.Rovers)
                    {
                        Console.WriteLine($"{rover.RoverPosition.X} {rover.RoverPosition.Y} {rover.RoverPosition.Direction}");
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("---------------");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Do you want to try again? (Y/N)");
                    var result = Console.ReadLine();
                    if (result.Equals("N", StringComparison.InvariantCultureIgnoreCase))
                    {
                        break;
                    }
                }

            } while (true);


            Console.WriteLine("---------------");
            Console.ReadKey();
        }


        static IServiceProvider Initialize()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.ConfigureApplication();
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;



        }


    }
}
