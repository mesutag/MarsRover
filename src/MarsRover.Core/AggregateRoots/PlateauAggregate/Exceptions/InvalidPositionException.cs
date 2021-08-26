using System;

namespace MarsRover.Core.AggregateRoots.PlateauAggregate.Exceptions
{
    public class InvalidPositionException : Exception
    {
        public InvalidPositionException(string reason) : base(reason)
        {
        }
    }
}
