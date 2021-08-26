using MarsRover.Core.AggregateRoots.PlateauAggregate.Exceptions;
using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;
using MarsRover.Core.SeedWork;
using System.Collections.Generic;
using System.Linq;

namespace MarsRover.Core.AggregateRoots.PlateauAggregate
{
    public class Rover : Entity
    {
        public RoverPosition Position { get; private set; }
        private readonly List<MovementDirection> _roverMovement;
        public IReadOnlyList<MovementDirection> RoverMovement => _roverMovement;

        protected Rover()
        {
            _roverMovement = new List<MovementDirection>();
        }
        public Rover(RoverPosition position) : this()
        {
            if (position == null)
                throw new InvalidPositionException("Invalid position");
            Position = position;
        }
        public void TurnLeft()
        {
            Direction direction = Position.Direction - 1 < Direction.N ? Direction.W : Position.Direction - 1;
            Position = new RoverPosition(Position.X, Position.Y, direction);
        }

        public void TurnRight()
        {
            Direction direction = Position.Direction + 1 > Direction.W ? Direction.N : Position.Direction + 1;
            Position = new RoverPosition(Position.X, Position.Y, direction);
        }

        public void Forward()
        {
            Position = Position.CalculateNextPosition();
        }
        public void AddDirections(List<MovementDirection> movementDirections)
        {
            _roverMovement.AddRange(movementDirections);
        }
    }

}
