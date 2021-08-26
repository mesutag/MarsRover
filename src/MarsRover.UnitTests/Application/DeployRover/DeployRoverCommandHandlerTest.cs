using MarsRover.Application;
using MarsRover.Application.Commands.DeployRover;
using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MarsRover.UnitTests.Application
{
    public class DeployRoverCommandHandlerTest
    {
        private readonly DeployRoverCommandValidator validator;
        private readonly Mock<IPlateauRepository> plateauRepository;
        public DeployRoverCommandHandlerTest()
        {
            plateauRepository = new Mock<IPlateauRepository>();
            validator = new DeployRoverCommandValidator();
        }
        [Theory]
        [InlineData(new object[] { 5, 5, "1 2 N", "LMLMLMLMM" })]
        [InlineData(new object[] { 5, 5, "3 3 N", "MMRMMRMRRM" })]
        public async Task DeployRover_Success(int plateauWidth, int plateauHeight, string roverPosition, string movementDirections)
        {
            Size size = new(plateauWidth, plateauHeight);
            Plateau plateau = new(size);

            plateauRepository.Setup(x => x.FindAsync(plateau.Id)).Returns(Task.FromResult(plateau));
            plateauRepository.Setup(x => x.UnitOfWork.SaveChangesAsync(default)).Returns(Task.FromResult(1));

            var command = new DeployRoverCommand(plateau.Id, roverPosition, movementDirections);
            var mediatr = new DeployRoverCommandHandler(plateauRepository.Object);
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
        public void Validator_When_Set_Invalid_RoverPosition_Then_Return_False(string roverPosition)
        {
            var command = new DeployRoverCommand(Guid.NewGuid(), roverPosition, "L");
            var validationResult = validator.Validate(command);
            Assert.False(validationResult.IsValid);
        }

        [Theory]
        [InlineData(new object[] { "1LMLMLMLMM" })]
        [InlineData(new object[] { "M2MRMMRM3RRM" })]
        [InlineData(new object[] { "LMLMLMLMM1" })]
        [InlineData(new object[] { "LMLM LMLMM" })]
        [InlineData(new object[] { "LRFTaLMRLMLRM" })]
        [InlineData(new object[] { "lrmrmrmrl" })]
        public void Validator_When_Set_Invalid_RoverDirection_Then_Return_False(string directions)
        {
            var command = new DeployRoverCommand(Guid.NewGuid(), "1 2 N", directions);
            var validationResult = validator.Validate(command);
            Assert.False(validationResult.IsValid);
        }
       

    }
}
