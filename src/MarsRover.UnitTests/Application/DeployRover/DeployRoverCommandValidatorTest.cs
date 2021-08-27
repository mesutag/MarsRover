using MarsRover.Application.Commands.DeployRover;
using MarsRover.Application.Commands.DeployRover;
using System;
using Xunit;

namespace MarsRover.UnitTests.Application.DeployRover
{
    public class DeployRoverCommandValidatorTest
    {
        private readonly DeployRoverCommandValidator validator;
        public DeployRoverCommandValidatorTest()
        {
            IDeployRoverCommandHelper deployRoverCommandHelper = new DeployRoverCommandHelper();
            validator = new DeployRoverCommandValidator(deployRoverCommandHelper);
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
