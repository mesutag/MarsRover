using MarsRover.Application.Model;
using MediatR;

namespace MarsRover.Application.Commands.CreatePlateau
{
    public class CreatePlateauCommand : IRequest<CommandResponse>
    {
        public string PlateauSize { get; internal set; }
        public CreatePlateauCommand(string plateauArea)
        {
            PlateauSize = plateauArea;
        }
    }
}
