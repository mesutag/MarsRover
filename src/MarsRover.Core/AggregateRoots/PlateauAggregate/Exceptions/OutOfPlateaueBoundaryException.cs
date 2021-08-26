using System;

namespace MarsRover.Core.AggregateRoots.PlateauAggregate.Exceptions
{
    public class OutOfPlateaueBoundaryException : Exception
    {
        public OutOfPlateaueBoundaryException(string reason) : base(reason)
        {
        }
    }
}
