using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;
using Xunit;

namespace MarsRover.UnitTests.Domain
{
    public class RoverTest
    {
        [Theory]
        [InlineData(new object[] { Direction.N, Direction.W })]
        [InlineData(new object[] { Direction.W, Direction.S })]
        [InlineData(new object[] { Direction.S, Direction.E })]
        [InlineData(new object[] { Direction.E, Direction.N })]
        public void TurnLeft_When_Entered_Direction_Return_Right_Directions(Direction inputDirection, Direction expected)
        {
            RoverPosition roverPosition = new(1, 2, inputDirection);
            Rover rover = new(roverPosition);
            rover.TurnLeft();

            Assert.Equal(expected, rover.Position.Direction);
        }
        [Theory]
        [InlineData(new object[] { Direction.N, Direction.E })]
        [InlineData(new object[] { Direction.E, Direction.S })]
        [InlineData(new object[] { Direction.S, Direction.W })]
        [InlineData(new object[] { Direction.W, Direction.N })]
        public void TurnRight_When_Entered_Direction_Then_Return_Right_Directions(Direction inputDirection, Direction expected)
        {
            RoverPosition roverPosition = new(1, 2, inputDirection);
            Rover rover = new(roverPosition);
            rover.TurnRight();

            Assert.Equal(expected, rover.Position.Direction);
        }
        [Theory]
        [InlineData(new object[] { 1, 2, Direction.N, 1, 3 })]
        [InlineData(new object[] { 1, 2, Direction.E, 2, 2 })]
        [InlineData(new object[] { 1, 2, Direction.W, 0, 2 })]
        public void Forward_When_Entered_Direction_Then_Success(int x, int y, Direction inputDirection, int expectedX, int expectedY)
        {
            RoverPosition roverPosition = new(x, y, inputDirection);
            Rover rover = new(roverPosition);
            rover.Forward();

            Assert.Equal(expectedX, rover.Position.X);
            Assert.Equal(expectedY, rover.Position.Y);
        }
    }


}
