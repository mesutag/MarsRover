using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;
using System.Text.RegularExpressions;

namespace MarsRover.Application.Commands.CreatePlateau
{
    public static class CreatePlateauCommandHelper
    {
        public static Size ParsePlateauSize(string plateauSize)
        {
            string regexPattern = @"^([0-9]+) ([0-9]+)$";
            Regex pos = new(regexPattern);

            var match = pos.Match(plateauSize);
            if (match.Success)
            {
                return new Size(int.Parse(match.Groups[1].Value),
                                    int.Parse(match.Groups[2].Value));
            }
            return null;
        }
    }
}
