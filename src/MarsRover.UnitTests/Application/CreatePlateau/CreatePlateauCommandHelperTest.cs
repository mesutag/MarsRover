using MarsRover.Application.Commands.CreatePlateau;
using Xunit;

namespace MarsRover.UnitTests.Application
{
    public class CreatePlateauCommandHelperTest
    {
        public CreatePlateauCommandHelperTest()
        {
        }

        [Theory]
        [InlineData(new object[] { "1" })]
        [InlineData(new object[] { "1 A" })]
        [InlineData(new object[] { "A A" })]
        [InlineData(new object[] { "A 1" })]
        [InlineData(new object[] { "1 2 3" })]
        public void GetPlateauSize_When_InvalidFormat_Return_Null(string input)
        {
            var actual = CreatePlateauCommandHelper.ParsePlateauSize(input);
            Assert.Null(actual);
        }
    }
}
