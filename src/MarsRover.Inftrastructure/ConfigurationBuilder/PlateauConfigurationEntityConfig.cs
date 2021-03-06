using MarsRover.Core.AggregateRoots.PlateauAggregate;
using Microsoft.EntityFrameworkCore;

namespace MarsRover.Inftrastructure.ConfigurationBuilder
{
    public static class PlateauConfigurationEntityConfig
    {
        public static void AddPlateauEntityConfigurations(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Plateau>(builder =>
            {
                builder.HasKey(p => p.Id);
                var navigation = builder.Metadata.FindNavigation(nameof(Plateau.DeployedRovers));
                navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
                navigation.SetIsEagerLoaded(true);
            });
            modelBuilder.Entity<Plateau>().OwnsOne(x => x.Size);
            modelBuilder.Entity<Rover>().OwnsOne(x => x.Position);

        }
    }
}
