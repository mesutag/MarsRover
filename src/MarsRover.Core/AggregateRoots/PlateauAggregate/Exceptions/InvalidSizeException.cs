using System;

namespace MarsRover.Core.AggregateRoots.PlateauAggregate.Exceptions
{
    public class InvalidSizeException : Exception
    {
        public InvalidSizeException(string reason) : base(reason)
        {
        }
    }
}
