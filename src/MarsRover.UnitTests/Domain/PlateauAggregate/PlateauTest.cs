using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MarsRover.Core.AggregateRoots.PlateauAggregate.Exceptions;
using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MarsRover.UnitTests.Domain
{
    public class PlateauTest
    {
        [Theory]
        [MemberData(nameof(MovementListData.Data), MemberType = typeof(MovementListData))]
        public void Execute_ExplorePlateau_Success(Size plateauSize, RoverPosition landedRoverPosition, List<MovementDirection> movementDirections, RoverPosition expectedPosition)
        {
            Plateau plateau = new(plateauSize);
            plateau.LandRover(landedRoverPosition, movementDirections);
            var rover = plateau.LandedRovers.First();

            plateau.ExplorePlateau(rover.Id);

            Assert.Equal(expectedPosition.X, rover.Position.X);
            Assert.Equal(expectedPosition.Y, rover.Position.Y);
            Assert.Equal(expectedPosition.Direction, rover.Position.Direction);
        }

        [Theory]
        [InlineData(new object[] { 0, 5 })]
        [InlineData(new object[] { 4, 0 })]
        public void PlateauSize_Invalid_Values_Throw_Exception(int width, int height)
        {
            Size size = new(width, height);
            Assert.Throws<InvalidSizeException>(() => new Plateau(size));
        }

        [Theory]
        [MemberData(nameof(PositionData.Data), MemberType = typeof(PositionData))]
        public void Rover_When_Land_RoverPositionValues_OutOf_PlateauBoundaries_Throw_Exception(Size plateauBoundariesSize, RoverPosition roverPosition)
        {
            Plateau plateau = new(plateauBoundariesSize);
            void actual() => plateau.LandRover(roverPosition, new List<MovementDirection> { });
            Assert.Throws<OutOfPlateaueBoundaryException>(actual);
        }
        [Theory]
        [InlineData(new object[] { 5, 5, 1, 5, Direction.N })]
        [InlineData(new object[] { 5, 5, 5, 5, Direction.N })]
        public void Rover_When_Next_RoverPositionValues_OutOf_PlateauBoundaries_Throw_Exception(int width, int height, int x, int y, Direction direction)
        {
            Plateau plateau = new(new Size(width, height));
            RoverPosition roverPosition = new(x, y, direction);
            plateau.LandRover(roverPosition, new List<MovementDirection> { MovementDirection.M });
            Rover rover = plateau.LandedRovers[0];
            List<RoverPosition> anotherRoverPositions = new();

            void actual() => plateau.CheckValidRoverMovement(rover, anotherRoverPositions);
            Assert.Throws<OutOfPlateaueBoundaryException>(actual);
        }
        [Theory]
        [InlineData(new object[] { 5, 5, 1, 2, Direction.E, 2, 2 })]
        [InlineData(new object[] { 5, 5, 3, 3, Direction.N, 3, 4 })]
        [InlineData(new object[] { 5, 5, 3, 3, Direction.S, 3, 2 })]
        [InlineData(new object[] { 5, 5, 2, 1, Direction.W, 1, 1 })]
        public void Rover_When_Next_RoverPositionValues_Conflict_AnotherRover_Throw_Exception(int width, int height, int x, int y, Direction direction, int anotherRoverPositionX, int anotherRoverPositionY)
        {
            Plateau plateau = new(new Size(width, height));
            RoverPosition roverPosition = new(x, y, direction);
            plateau.LandRover(roverPosition, new List<MovementDirection> { MovementDirection.M });

            RoverPosition anotherRoverPosition = new(anotherRoverPositionX, anotherRoverPositionY, Direction.N);
            plateau.LandRover(anotherRoverPosition, new List<MovementDirection> { MovementDirection.M });

            Rover rover = plateau.LandedRovers[0];
            void actual() => plateau.CheckValidRoverMovement(rover, new List<RoverPosition>() { anotherRoverPosition });
            Assert.Throws<ConflictException>(actual);
        }

        [Theory]
        [InlineData(new object[] { 5, 5, 1, 2, Direction.N, MovementDirection.L, 1, 2, Direction.W })]
        [InlineData(new object[] { 5, 5, 1, 2, Direction.N, MovementDirection.R, 1, 2, Direction.E })]
        [InlineData(new object[] { 5, 5, 1, 2, Direction.N, MovementDirection.M, 1, 3, Direction.N })]
        [InlineData(new object[] { 5, 5, 1, 2, Direction.W, MovementDirection.M, 0, 2, Direction.W })]
        [InlineData(new object[] { 5, 5, 3, 2, Direction.S, MovementDirection.L, 3, 2, Direction.E })]
        [InlineData(new object[] { 5, 5, 3, 2, Direction.S, MovementDirection.R, 3, 2, Direction.W })]
        public void ExplorePlatueau_Direction_There_Is_No_AnotherRover_Return_Expected(int plateauWidth, int plateauHeight, int x, int y, Direction inputDirection, MovementDirection movementDirection, int expectedX, int expectedY, Direction expectedDirection)
        {
            Size size = new(plateauWidth, plateauHeight);
            Plateau plateau = new(size);
            RoverPosition roverPosition = new(x, y, inputDirection);
            plateau.LandRover(roverPosition, new List<MovementDirection> { movementDirection });
            var rover = plateau.LandedRovers[0];

            plateau.ExplorePlateau(rover.Id);

            Assert.Equal(expectedDirection, rover.Position.Direction);
            Assert.Equal(expectedX, rover.Position.X);
            Assert.Equal(expectedY, rover.Position.Y);
        }

        [Theory]
        [InlineData(new object[] { 1, 2, Direction.W, 1, 2 })]
        [InlineData(new object[] { 4, 2, Direction.E, 4, 2 })]
        [InlineData(new object[] { 3, 2, Direction.N, 3, 2 })]
        [InlineData(new object[] { 3, 2, Direction.S, 3, 2 })]
        public void AnyAnotherRoverDestinationPosition_Return_False(int x, int y, Direction inputDirection,
           int anotherRoverPositionX, int anotherRoverPositionY)
        {
            Size size = new(5, 5);
            Plateau plateau = new(size);

            List<RoverPosition> otherRoverPositions = new()
            {
                new(anotherRoverPositionX, anotherRoverPositionY, inputDirection)
            };


            var actual = plateau.AnyAnotherRoverDestinationPosition(x, y, otherRoverPositions);
            Assert.True(actual);
        }

        [Theory]
        [InlineData(new object[] { 5, 5, -1, 2 })]
        [InlineData(new object[] { 5, 5, 6, 2 })]
        [InlineData(new object[] { 5, 5, 1, 6 })]
        [InlineData(new object[] { 5, 5, 1, -1 })]
        public void IsOutOfBoundariesLandedPosition_Return_True(
            int plateauWidth, int plateauHeight, int x, int y)
        {
            Size plateauSize = new(plateauWidth, plateauHeight);
            Plateau plateau = new(plateauSize);


            var actual = plateau.IsOutOfBoundariesTheDestinationPosition(x, y);

            Assert.True(actual);
        }

        public record PositionData
        {
            public static IEnumerable<object[]> Data =>
                    new List<object[]>
                    {
                    new object[] { new Size(5, 5),new RoverPosition(1,6,Direction.N)},
                    new object[] { new Size(4, 5),new RoverPosition(5,3,Direction.E)},
                    new object[] { new Size(4, 5),new RoverPosition(3,-1,Direction.E)},
                    new object[] { new Size(4, 5),new RoverPosition(-1,3,Direction.E)},
                    };
        }


        public record MovementListData
        {
            public static IEnumerable<object[]> Data =>
                    new List<object[]>
                    {
                        new object[]{
                            new Size(5, 5),
                             new RoverPosition(1,2 ,Direction.N),
                            new List<MovementDirection>()
                                {
                                    MovementDirection.L,
                                    MovementDirection.M,
                                    MovementDirection.L,
                                    MovementDirection.M,
                                    MovementDirection.L,
                                    MovementDirection.M,
                                    MovementDirection.L,
                                    MovementDirection.M,
                                    MovementDirection.M
                                },
                             new RoverPosition(1,3 ,Direction.N)
                        },
                        new object[]{
                            new Size(5, 5),
                            new RoverPosition(3,3 ,Direction.E),
                            new List<MovementDirection>()
                                {
                                    MovementDirection.M,
                                    MovementDirection.M,
                                    MovementDirection.R,
                                    MovementDirection.M,
                                    MovementDirection.M,
                                    MovementDirection.R,
                                    MovementDirection.M,
                                    MovementDirection.R,
                                    MovementDirection.R,
                                    MovementDirection.M
                                },
                            new RoverPosition(5,1,Direction.E)
                        },
                    };
        }
    }


}
