using System;

namespace MarsRover.Core.AggregateRoots.PlateauAggregate.Exceptions
{
    public class OutOfBoundaryException : Exception
    {
        public OutOfBoundaryException(string reason) : base(reason)
        {
        }
    }
}
