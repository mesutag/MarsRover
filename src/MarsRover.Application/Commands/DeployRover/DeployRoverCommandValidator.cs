using FluentValidation;

namespace MarsRover.Application.Commands.DeployRover
{
    public class DeployRoverCommandValidator : AbstractValidator<DeployRoverCommand>
    {
        public DeployRoverCommandValidator(IDeployRoverCommandHelper deployRoverCommandHelper)
        {
            RuleFor(x => x.RoverPosition).NotEmpty()
                   .WithMessage("Please enter the rover positions!");
            RuleFor(x => x.RoverDirections).NotEmpty()
                   .WithMessage("Please enter the rover directions!");
            RuleFor(x => x.RoverPosition).Must(p => deployRoverCommandHelper.ParsePosition(p) != null)
                   .WithMessage("The position input is not in the expected format.");
            RuleFor(x => x.RoverDirections).Must(p => deployRoverCommandHelper.ParseDirections(p) != null)
                  .WithMessage("This movement input is not valid.");
        }

    }

}
