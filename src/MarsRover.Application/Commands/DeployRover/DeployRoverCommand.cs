using MarsRover.Application.Model;
using MediatR;
using System;

namespace MarsRover.Application.Commands.DeployRover
{
    public class DeployRoverCommand : IRequest<CommandResponse>
    {
        public Guid PlateauId { get; internal set; }
        public string RoverPosition { get; internal set; }
        public string RoverDirections { get; set; }
        public DeployRoverCommand(Guid plateauId, string roverPosition, string roverDirections)
        {
            PlateauId = plateauId;
            RoverPosition = roverPosition;
            RoverDirections = roverDirections;
        }
    }
}
