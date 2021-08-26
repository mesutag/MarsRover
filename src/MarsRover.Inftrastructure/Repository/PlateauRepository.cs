using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MarsRover.Core.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MarsRover.Inftrastructure.Repository
{
    public class PlateauRepository : IPlateauRepository
    {
        private readonly MarsRoverContext marsRoverContext;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return marsRoverContext;
            }
        }
        public PlateauRepository(MarsRoverContext marsRoverContext)
        {
            this.marsRoverContext = marsRoverContext;
        }

        public async Task<Plateau> FindAsync(Guid plateauId)
        {
            return await marsRoverContext.Plateau
                                           .FirstOrDefaultAsync(p => p.Id == plateauId);
        }

        public void UpdatePlateau(Plateau plateau)
        {
            marsRoverContext.Entry(plateau).State = EntityState.Modified;
        }

        public async Task CreatePlateauAsync(Plateau plateau)
        {
            await marsRoverContext.Plateau.AddAsync(plateau);
        }
    }
}
