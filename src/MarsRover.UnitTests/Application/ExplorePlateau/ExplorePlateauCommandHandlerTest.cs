using MarsRover.Application;
using MarsRover.Application.Commands.ExplorePlateau;
using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MarsRover.UnitTests.Application.ExplorePlateau
{
    public class ExplorePlateauCommandHandlerTest
    {
        private readonly Mock<IPlateauRepository> plateauRepository;
        public ExplorePlateauCommandHandlerTest()
        {
            plateauRepository = new Mock<IPlateauRepository>();
        }
        [Fact]
        public async Task ExplorePlateau_Return_NotNull()
        {
            Size size = new(5, 5);
            Plateau plateau = new(size);
            RoverPosition roverPosition = new(1, 2, Direction.E);
            List<MovementDirection> movementDirections = new() { MovementDirection.L };
            plateau.DeployRover(roverPosition, movementDirections);
            var rover = plateau.DeployedRovers.First();

            plateauRepository.Setup(p => p.FindAsync(plateau.Id)).ReturnsAsync(plateau);
            plateauRepository.Setup(p => p.UnitOfWork.SaveChangesAsync(default)).Returns(Task.FromResult(1));


            var command = new ExplorePlateauCommand(plateau.Id, rover.Id);
            var handler = new ExplorePlateauCommandHandler(plateauRepository.Object);
            var result = await handler.Handle(command, CancellationToken.None);




            plateauRepository.Verify(x => x.FindAsync(It.IsAny<Guid>()));
            plateauRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(default));
            Assert.NotNull(result);
        }
    }
}
