using MarsRover.Application;
using MarsRover.Application.Commands.CreatePlateau;
using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MarsRover.UnitTests.Application.CreatePlateau
{
    public class CreatePlateauCommandHandlerTest
    {
        private readonly Mock<ICreatePlateauCommandHelper> createPlateauCommandHelper;
        private readonly Mock<IPlateauRepository> plateauRepository;
        public CreatePlateauCommandHandlerTest()
        {
            plateauRepository = new Mock<IPlateauRepository>();
            createPlateauCommandHelper = new Mock<ICreatePlateauCommandHelper>();
        }
        [Theory]
        [InlineData(new object[] { "3 5", 3, 5 })]
        [InlineData(new object[] { "8 3", 8, 3 })]
        [InlineData(new object[] { "10 10", 10, 10 })]
        public async Task CreatePlateau_When_ValidFormat_Return_NotNull(string input, int width, int height)
        {
            plateauRepository.Setup(plateau => plateau.UnitOfWork.SaveChangesAsync(default)).Returns(Task.FromResult(1));

            var command = new CreatePlateauCommand(input);
            CreatePlateauCommandValidator validationRules = new(new CreatePlateauCommandHelper());
            var validationResult = validationRules.Validate(command).Errors.ToList();
            createPlateauCommandHelper.Setup(p => p.ParsePlateauSize(input))
               .Returns(() =>
               {
                   if (validationResult.Any())
                       CustomApplicationException.ThrowValidationException(validationResult);
                   return new Size(width, height);
               });



            var handler = new CreatePlateauCommandHandler(plateauRepository.Object, createPlateauCommandHelper.Object);
            var result = await handler.Handle(command, CancellationToken.None);



            plateauRepository.Verify(x => x.CreatePlateauAsync(It.IsAny<Plateau>()));
            plateauRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(default));

            Assert.NotNull(result);
        }


        [Theory]
        [InlineData(new object[] { "35" })]
        [InlineData(new object[] { "8" })]
        [InlineData(new object[] { "1010" })]
        [InlineData(new object[] { "10N10" })]
        [InlineData(new object[] { "L M" })]
        [InlineData(new object[] { "1 2 N" })]
        [InlineData(new object[] { "RR" })]
        [InlineData(new object[] { "1 N" })]
        [InlineData(new object[] { "" })]
        public async Task CreatePlateau_InvalidFormat_Throw_Exception(string input)
        {
            var command = new CreatePlateauCommand(input);
            CreatePlateauCommandValidator validationRules = new(new CreatePlateauCommandHelper());
            var validationResult = validationRules.Validate(command).Errors.ToList();
            createPlateauCommandHelper.Setup(p => p.ParsePlateauSize(input))
                .Returns(() =>
                {
                    if (validationResult.Any())
                        CustomApplicationException.ThrowValidationException(validationResult);
                    return new Size(It.IsAny<int>(), It.IsAny<int>());
                });




            var handler = new CreatePlateauCommandHandler(plateauRepository.Object, createPlateauCommandHelper.Object);
            Task func() => handler.Handle(command, CancellationToken.None);

            await Assert.ThrowsAsync<CustomApplicationException>(func);
        }
    }
}
