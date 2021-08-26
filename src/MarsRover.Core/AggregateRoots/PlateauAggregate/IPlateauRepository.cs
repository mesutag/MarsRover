using MarsRover.Core.SeedWork;
using System;
using System.Threading.Tasks;

namespace MarsRover.Core.AggregateRoots.PlateauAggregate
{
    public interface IPlateauRepository : IRepository<Plateau>
    {
        Task CreatePlateauAsync(Plateau plateau);
        Task<Plateau> FindAsync(Guid plateauId);
        void UpdatePlateau(Plateau plateau);
    }
}
