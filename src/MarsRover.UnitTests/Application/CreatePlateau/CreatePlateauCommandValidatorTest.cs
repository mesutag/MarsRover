using MarsRover.Application.Commands.CreatePlateau;
using Xunit;

namespace MarsRover.UnitTests.Application
{
    public class CreatePlateauCommandValidatorTest
    {
        private readonly CreatePlateauCommandValidator validator;
        public CreatePlateauCommandValidatorTest()
        {
            validator = new CreatePlateauCommandValidator();
        }

        [Theory]
        [InlineData(new object[] { "" })]
        [InlineData(new object[] { "1" })]
        [InlineData(new object[] { "1 A" })]
        [InlineData(new object[] { "A A" })]
        [InlineData(new object[] { "A 1" })]
        [InlineData(new object[] { "1 2 3" })]
        public void GetPlateauSize_When_InvalidFormat_Return_False(string input)
        {
            var command = new CreatePlateauCommand(input);
            var validationResult = validator.Validate(command);
            Assert.False(validationResult.IsValid);
        }

    }
}
