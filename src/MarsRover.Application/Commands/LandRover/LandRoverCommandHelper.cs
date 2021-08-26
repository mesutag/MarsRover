using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MarsRover.Application.Commands.LandRover
{
    public class LandRoverCommandHelper : ILandRoverCommandHelper
    {
        public RoverPosition ParsePosition(string roverPositionInput)
        {
            string regexPattern = @"^([0-9]+) ([0-9]+) ([NSEW])$";
            Regex pos = new(regexPattern);

            var match = pos.Match(roverPositionInput);
            if (match.Success)
            {
                return new RoverPosition(int.Parse(match.Groups[1].Value),
                                    int.Parse(match.Groups[2].Value),
                                    (Direction)Enum.Parse(typeof(Direction), match.Groups[3].Value));
            }
            return null;
        }
        public List<MovementDirection> ParseDirections(string inputDirections)
        {
            List<MovementDirection> movementDirections = new();
            string regexPattern = @"^[LRM]+$";
            Regex pos = new(regexPattern);

            var match = pos.Match(inputDirections);
            if (match.Success)
            {
                var directionArray = inputDirections.ToCharArray();
                foreach (var direction in directionArray)
                {
                    Enum.TryParse(direction.ToString(), out MovementDirection movementDirection);
                    movementDirections.Add(movementDirection);
                }
                return movementDirections;
            }
            return null;


        }
    }
}
