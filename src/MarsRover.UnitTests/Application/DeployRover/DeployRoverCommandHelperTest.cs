using MarsRover.Application.Commands.DeployRover;
using Xunit;

namespace MarsRover.UnitTests.Application.DeployRover
{
    public class DeployRoverCommandHelperTest
    {
        private readonly IDeployRoverCommandHelper deployRoverCommandHelper;
        public DeployRoverCommandHelperTest()
        {
            deployRoverCommandHelper = new DeployRoverCommandHelper();
        }

        [Theory]
        [InlineData(new object[] { "" })]
        [InlineData(new object[] { "1" })]
        [InlineData(new object[] { "1 2" })]
        [InlineData(new object[] { "3 E" })]
        [InlineData(new object[] { "N E W" })]
        [InlineData(new object[] { "N E 1" })]
        [InlineData(new object[] { "E 1 N" })]
        [InlineData(new object[] { "1 1 1" })]
        [InlineData(new object[] { "1 1 V" })]
        [InlineData(new object[] { "1 1 N E" })]
        public void GetPosition_When_Set_Invalid_RoverPosition_Then_Return_Null(string roverPosition)
        {
            var actual = deployRoverCommandHelper.ParsePosition(roverPosition);
            Assert.Null(actual);
        }

        [Theory]
        [InlineData(new object[] { "" })]
        [InlineData(new object[] { "2324324" })]
        [InlineData(new object[] { "1LMLMLMLMM" })]
        [InlineData(new object[] { "M2MRMMRM3RRM" })]
        [InlineData(new object[] { "LMLMLMLMM1" })]
        [InlineData(new object[] { "LMLM LMLMM" })]
        [InlineData(new object[] { "LRFTaLMRLMLRM" })]
        [InlineData(new object[] { "lrmrmrmrl" })]
        public void GetDirections_When_Set_Invalid_RoverDirection_Then_Return_Null(string directions)
        {
            var actual = deployRoverCommandHelper.ParseDirections(directions);
            Assert.Null(actual);
        }


    }
}
