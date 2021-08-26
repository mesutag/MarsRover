using System;

namespace MarsRover.Application.Queries.GetPlateauById
{
    public record RoverModel
    {
        public Guid RoverId { get; set; }
        public RoverPositionModel RoverPosition { get; set; }
    }
}
