using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;

namespace MarsRover.Application.Queries.GetPlateauById
{
    public class RoverPositionModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Direction { get; set; }
    }
}
