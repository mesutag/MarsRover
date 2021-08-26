using MarsRover.Application;
using MarsRover.Application.Commands;
using MarsRover.Application.Commands.CreatePlateau;
using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MarsRover.Core.SeedWork;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MarsRover.UnitTests.Application
{
    public class CreatePlateauCommandHandlerTest
    {
        private readonly Mock<IPlateauRepository> plateauRepository;
        public CreatePlateauCommandHandlerTest()
        {
            plateauRepository = new Mock<IPlateauRepository>();
        }
        [Theory]
        [InlineData(new object[] { "3 5" })]
        [InlineData(new object[] { "8 3" })]
        [InlineData(new object[] { "10 10" })]
        public async Task CreatePlateau_When_ValidFormat_Return_NotNull(string input)
        {
            plateauRepository.Setup(plateau => plateau.UnitOfWork.SaveChangesAsync(default)).Returns(Task.FromResult(1));

            var command = new CreatePlateauCommand(input);
            var mediatr = new CreatePlateauCommandHandler(plateauRepository.Object);
            var result = await mediatr.Handle(command, CancellationToken.None);

            plateauRepository.Verify(x => x.CreatePlateauAsync(It.IsAny<Plateau>()));
            plateauRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(default));

            Assert.NotNull(result);
        }
    }
}
