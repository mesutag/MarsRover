using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MarsRover.Core.SeedWork;
using MarsRover.Inftrastructure.ConfigurationBuilder;
using Microsoft.EntityFrameworkCore;

namespace MarsRover.Inftrastructure
{
    public class MarsRoverContext : DbContext, IUnitOfWork
    {
        public MarsRoverContext(DbContextOptions<MarsRoverContext> options) : base(options)
        {

        }
        public DbSet<Plateau> Plateau { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddPlateauEntityConfigurations();
            base.OnModelCreating(modelBuilder);
        }

    }
}
