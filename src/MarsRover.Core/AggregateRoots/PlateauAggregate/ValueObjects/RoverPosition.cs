namespace MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects
{
    public class RoverPosition
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Direction Direction { get; private set; }

        public RoverPosition(int x = 0, int y = 0, Direction direction = Direction.N)
        {
            X = x;
            Y = y;
            Direction = direction;
        }
        public RoverPosition CalculateNextPosition()
        {
            RoverPosition roverPosition = this;
            switch (Direction)
            {
                case Direction.N:
                    roverPosition = new RoverPosition(X, Y + 1, Direction);
                    break;
                case Direction.E:
                    roverPosition = new RoverPosition(X + 1, Y, Direction);
                    break;
                case Direction.S:
                    roverPosition = new RoverPosition(X, Y - 1, Direction);
                    break;
                case Direction.W:
                    roverPosition = new RoverPosition(X - 1, Y, Direction);
                    break;
            }
            return roverPosition;
        }
    }
}
