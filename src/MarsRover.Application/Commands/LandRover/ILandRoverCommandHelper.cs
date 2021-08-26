using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;
using System.Collections.Generic;

namespace MarsRover.Application.Commands.LandRover
{
    public interface ILandRoverCommandHelper
    {
        RoverPosition ParsePosition(string roverPositionInput);
        List<MovementDirection> ParseDirections(string inputDirections);
    }
}
