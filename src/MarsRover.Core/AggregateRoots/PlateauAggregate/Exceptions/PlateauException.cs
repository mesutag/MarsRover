using System;

namespace MarsRover.Core.AggregateRoots.PlateauAggregate.Exceptions
{
    public class PlateauException : Exception
    {
        public PlateauException(string reason) : base(reason)
        {
        }
    }
    public class InvalidSizeException : Exception
    {
        public InvalidSizeException(string reason) : base(reason)
        {
        }
    }
    public class ConflictException : Exception
    {
        public ConflictException(string reason) : base(reason)
        {
        }
    }
    public class OutOfBoundaryException : Exception
    {
        public OutOfBoundaryException(string reason) : base(reason)
        {
        }
    }
}
