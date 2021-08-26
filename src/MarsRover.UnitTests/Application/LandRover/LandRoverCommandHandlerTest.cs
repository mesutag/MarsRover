using MarsRover.Application;
using MarsRover.Application.Behaivor;
using MarsRover.Application.Commands.LandRover;
using MarsRover.Application.Commands.LandRover;
using MarsRover.Application.Model;
using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;
using MarsRover.Core.SeedWork;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MarsRover.UnitTests.Application.LandRover
{
    public class LandRoverCommandHandlerTest
    {
        private readonly LandRoverCommandValidator validator;
        private readonly Mock<IPlateauRepository> plateauRepository;
        private readonly Mock<ILandRoverCommandHelper> landRoverCommandHelper;
        public LandRoverCommandHandlerTest()
        {
            plateauRepository = new Mock<IPlateauRepository>();
            landRoverCommandHelper = new Mock<ILandRoverCommandHelper>();
            validator = new LandRoverCommandValidator(landRoverCommandHelper.Object);
        }
        [Theory]
        [InlineData(new object[] { 5, 5, "1 2 N", "LMLMLMLMM" })]
        [InlineData(new object[] { 5, 5, "3 3 N", "MMRMMRMRRM" })]
        public async Task LandRover_Success(int plateauWidth, int plateauHeight, string roverPosition, string movementDirections)
        {
            Size size = new(plateauWidth, plateauHeight);
            Plateau plateau = new(size);

            plateauRepository.Setup(x => x.FindAsync(plateau.Id)).Returns(Task.FromResult(plateau));
            plateauRepository.Setup(x => x.UnitOfWork.SaveChangesAsync(default)).Returns(Task.FromResult(1));

            var command = new LandRoverCommand(plateau.Id, roverPosition, movementDirections);
            //RequestValidationBehavior fluent validation step
            LandRoverCommandValidator validationRules = new(new LandRoverCommandHelper());
            var validationResult = validationRules.Validate(command).Errors.ToList();
            landRoverCommandHelper.Setup(p => p.ParsePosition(roverPosition))
               .Returns(() =>
               {
                   if (validationResult.Any())
                       CustomApplicationException.ThrowValidationException(validationResult);
                   return new RoverPosition(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Direction>());
               });

            landRoverCommandHelper.Setup(p => p.ParseDirections(movementDirections))
               .Returns(() =>
               {
                   if (validationResult.Any())
                       CustomApplicationException.ThrowValidationException(validationResult);
                   return new List<MovementDirection>();
               });





            var handler = new LandRoverCommandHandler(plateauRepository.Object, landRoverCommandHelper.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(new object[] { "12 N", "LMLMLMLMM" })]
        [InlineData(new object[] { "3 3 N", "MMRMM RMRRM" })]
        [InlineData(new object[] { "", "MMRMMRMRRM" })]
        [InlineData(new object[] { "", "" })]
        [InlineData(new object[] { "3 3 N", "" })]
        [InlineData(new object[] { "MMRMMRMRRM", "3 3 N" })]
        public async Task LandRover_Invalid_Format_Throw_Exception(string roverPosition, string movementDirections)
        {
            Size size = new(5, 5);
            Plateau plateau = new(size);
            var command = new LandRoverCommand(plateau.Id, roverPosition, movementDirections);
            var handler = new LandRoverCommandHandler(plateauRepository.Object, landRoverCommandHelper.Object);
            //RequestValidationBehavior fluent validation step
            LandRoverCommandValidator validationRules = new(new LandRoverCommandHelper());
            var validationResult = validationRules.Validate(command).Errors.ToList();
            landRoverCommandHelper.Setup(p => p.ParsePosition(roverPosition))
               .Returns(() =>
               {
                   if (validationResult.Any())
                       CustomApplicationException.ThrowValidationException(validationResult);
                   return new RoverPosition(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Direction>());
               });

            landRoverCommandHelper.Setup(p => p.ParseDirections(movementDirections))
               .Returns(() =>
               {
                   if (validationResult.Any())
                       CustomApplicationException.ThrowValidationException(validationResult);
                   return new List<MovementDirection>();
               });


            Task func() => handler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<CustomApplicationException>(func);
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
            var command = new LandRoverCommand(Guid.NewGuid(), roverPosition, "L");
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
            var command = new LandRoverCommand(Guid.NewGuid(), "1 2 N", directions);
            var validationResult = validator.Validate(command);
            Assert.False(validationResult.IsValid);
        }


    }
}
