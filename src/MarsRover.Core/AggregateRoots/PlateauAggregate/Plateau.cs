using MarsRover.Core.AggregateRoots.PlateauAggregate.Exceptions;
using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;
using MarsRover.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRover.Core.AggregateRoots.PlateauAggregate
{
    public class Plateau : Entity, IAggregateRoot
    {
        public Size Size { get; private set; }
        private readonly List<Rover> _deployedRovers;
        public IReadOnlyList<Rover> DeployedRovers => _deployedRovers;

        protected Plateau()
        {
            _deployedRovers = new List<Rover>();
        }
        public Plateau(Size size) : this()
        {
            SetSize(size);
        }

        public void DeployRover(RoverPosition position, List<MovementDirection> movementDirections)
        {
            Rover rover = new(position);
            if (AnyAnotherRoverDestinationPosition(position.X, position.Y, GetRoverPositionsOnPlateau(rover.Id)))
                throw new ConflictException($"There is another rover at deployed position X:{position.X} Y:{position.Y}!");
            else if (IsOutOfBoundariesTheDestinationPosition(position.X, position.Y))
                throw new OutOfPlateaueBoundaryException($"Deployed position is out of plateau boundaries! Plateu area is width {Size.Width} and height {Size.Height}");


            rover.AddDirections(movementDirections);

            _deployedRovers.Add(rover);
        }
        public void ExplorePlateau(Guid roverId)
        {
            var rover = GetRover(roverId);

            foreach (var movement in rover.RoverMovement)
            {
                switch (movement)
                {
                    case MovementDirection.L:
                        rover.TurnLeft();
                        break;
                    case MovementDirection.R:
                        rover.TurnRight();
                        break;
                    case MovementDirection.M:
                        var anotherRoverPositions = GetRoverPositionsOnPlateau(rover.Id);
                        CheckValidRoverMovement(rover, anotherRoverPositions);
                        rover.Forward();
                        break;
                }
            }
        }
        public void CheckValidRoverMovement(Rover rover, List<RoverPosition> anotherRoverPositions)
        {
            RoverPosition nextPosition = rover.Position.CalculateNextPosition();

            if (AnyAnotherRoverDestinationPosition(nextPosition.X, nextPosition.Y, anotherRoverPositions))
                throw new ConflictException("Crash Alert! There is another rover at movement position.");
            else if (IsOutOfBoundariesTheDestinationPosition(nextPosition.X, nextPosition.Y))
                throw new OutOfPlateaueBoundaryException("Rover's movement position is out of plateau boundaries!");
        }
        private Rover GetRover(Guid roverId)
        {
            return _deployedRovers.FirstOrDefault(p => p.Id == roverId);
        }

        private void SetSize(Size size)
        {
            if (size.Height < 1 || size.Width < 1)
                throw new InvalidSizeException("Enter valid values.");

            Size = size;
        }
        private List<RoverPosition> GetRoverPositionsOnPlateau(Guid excludedRoverId)
        {
            return DeployedRovers.Where(p => p.Id != excludedRoverId).Select(p => p.Position).ToList();
        }
        public bool AnyAnotherRoverDestinationPosition(int x, int y, List<RoverPosition> anotherRoverPositions)
        {
            return anotherRoverPositions.Any(p => p.Y == y && p.X == x);
        }
        public bool IsOutOfBoundariesTheDestinationPosition(int x, int y)
        {
            return y < 0 || y > Size.Height || x < 0 || x > Size.Width;
        }
    }

}
