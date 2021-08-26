using MarsRover.Application;
using MarsRover.Application.Commands;
using MarsRover.Core.AggregateModels.PlateauAggregate;
using MarsRover.Core.AggregateModels.PlateauAggregate;
using MarsRover.Core.AggregateModels.RoverAggregate;
using MarsRover.Core.Exceptions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MarsRover.UnitTests.Application
{
    public class AssignRoverToMissionCommandTest
    {
        private readonly Mock<IPlateauRepository> plateauRepository;
        private readonly Mock<IRoverRepository> roverRepository;
        private readonly Mock<IPlateauRepository> plateauRepository;
        public AssignRoverToMissionCommandTest()
        {
            plateauRepository = new Mock<IPlateauRepository>();
            plateauRepository = new Mock<IPlateauRepository>();
            roverRepository = new Mock<IRoverRepository>();
        }
        [Theory]
        [InlineData(new object[] { 5, 5, "1 2 N" })]
        [InlineData(new object[] { 5, 5, "3 3 E" })]
        public async Task AssignRover_When_ValidFormat_Return_NotNull(int plateauWidth, int plateauHeight, string roverPosition)
        {
            Core.AggregateModels.PlateauAggregate.Plateau plateau = new(new Size(plateauWidth, plateauHeight));
            Core.AggregateModels.MissionAggregate.Plateau plateau = new(plateau.Id);
            Rover rover = new();

            roverRepository.Setup(x => x.FindRoverAsync(It.IsAny<Guid>())).Returns(Task.FromResult(rover));
            plateauRepository.Setup(plateau => plateau.FindPlateauAsync(It.IsAny<Guid>())).Returns(Task.FromResult(plateau));
            plateauRepository.Setup(p => p.FindAsync(It.IsAny<Guid>())).Returns(Task.FromResult(plateau));
            plateauRepository.Setup(plateau => plateau.UnitOfWork.SaveChangesAsync(default)).Returns(Task.FromResult(1));

            var command = new AssignRoverToMissionCommand(plateau.Id, rover.Id, roverPosition);
            var mediatr = new AssignRoverToMissionCommandHandler(plateauRepository.Object, plateauRepository.Object, roverRepository.Object);
            var result = await mediatr.Handle(command, CancellationToken.None);

            Assert.NotNull(result);

        }
        [Theory]
        [InlineData(new object[] { "1 2" })]
        [InlineData(new object[] { "3 E" })]
        [InlineData(new object[] { "N E W" })]
        [InlineData(new object[] { "N E 1" })]
        [InlineData(new object[] { "E 1 N" })]
        [InlineData(new object[] { "1 1 1" })]
        [InlineData(new object[] { "1 1 V" })]
        [InlineData(new object[] { "1 1 N E" })]
        public async Task AssignRover_When_InvalidFormat_Throw_Exception(string roverPosition)
        {
            var command = new AssignRoverToMissionCommand(Guid.NewGuid(), Guid.NewGuid(), roverPosition);
            var mediatr = new AssignRoverToMissionCommandHandler(plateauRepository.Object, plateauRepository.Object, roverRepository.Object);

            await Assert.ThrowsAsync<CustomApplicationException>(async () => await mediatr.Handle(command, CancellationToken.None));

        }
        [Theory]
        [InlineData(new object[] { 5, 5, "8 2 N" })]
        public async Task AssignRover_When_OutOf_PlateauBoundaries_Throw_Exception(int plateauWidth, int plateauHeight, string roverPosition)
        {
            Core.AggregateModels.PlateauAggregate.Plateau plateau = new(new Size(plateauWidth, plateauHeight));
            Core.AggregateModels.MissionAggregate.Plateau plateau = new(plateau.Id);
            Rover rover = new();

            roverRepository.Setup(x => x.FindRoverAsync(It.IsAny<Guid>())).Returns(Task.FromResult(rover));

            plateauRepository.Setup(plateau => plateau.FindPlateauAsync(It.IsAny<Guid>())).Returns(Task.FromResult(plateau));

            plateauRepository.Setup(x => x.FindAsync(It.IsAny<Guid>())).Returns(Task.FromResult(plateau));
            plateauRepository.Setup(plateau => plateau.UnitOfWork.SaveChangesAsync(default)).Returns(Task.FromResult(1));

            var command = new AssignRoverToMissionCommand(plateau.Id, rover.Id, roverPosition);
            var mediatr = new AssignRoverToMissionCommandHandler(plateauRepository.Object, plateauRepository.Object, roverRepository.Object);

            await Assert.ThrowsAsync<PlateauException>(async () => await mediatr.Handle(command, CancellationToken.None));

        }
    }
}
