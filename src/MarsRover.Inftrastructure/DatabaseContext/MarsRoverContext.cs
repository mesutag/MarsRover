using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MarsRover.Core.SeedWork;
using MarsRover.Inftrastructure.ConfigurationBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MarsRover.Inftrastructure.DatabaseContext
{
    public class MarsRoverContext : DbContext, IUnitOfWork

    {
        private readonly IOptions<MarsRoverContextOption> options;
        public MarsRoverContext(DbContextOptions<MarsRoverContext> dbContextOptions, IOptions<MarsRoverContextOption> options) : base(dbContextOptions)
        {
            this.options = options;
        }
        public DbSet<Plateau> Plateau { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: options.Value.DatabaseName);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddPlateauEntityConfigurations();
            base.OnModelCreating(modelBuilder);
        }

    }
}
