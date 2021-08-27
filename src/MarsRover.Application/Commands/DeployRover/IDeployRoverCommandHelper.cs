using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;
using System.Collections.Generic;

namespace MarsRover.Application.Commands.DeployRover
{
    public interface IDeployRoverCommandHelper
    {
        RoverPosition ParsePosition(string roverPositionInput);
        List<MovementDirection> ParseDirections(string inputDirections);
    }
}
