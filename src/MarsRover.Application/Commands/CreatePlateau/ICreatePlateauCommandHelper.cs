using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;

namespace MarsRover.Application.Commands.CreatePlateau
{
    public interface ICreatePlateauCommandHelper
    {
        Size ParsePlateauSize(string plateauSize);
    }
    
}
