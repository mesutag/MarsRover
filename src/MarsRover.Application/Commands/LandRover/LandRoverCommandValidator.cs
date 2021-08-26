using FluentValidation;

namespace MarsRover.Application.Commands.LandRover
{
    public class LandRoverCommandValidator : AbstractValidator<LandRoverCommand>
    {
        public LandRoverCommandValidator(ILandRoverCommandHelper landRoverCommandHelper)
        {
            RuleFor(x => x.RoverPosition).NotEmpty()
                   .WithMessage("Please enter the rover positions!");
            RuleFor(x => x.RoverDirections).NotEmpty()
                   .WithMessage("Please enter the rover directions!");
            RuleFor(x => x.RoverPosition).Must(p => landRoverCommandHelper.ParsePosition(p) != null)
                   .WithMessage("The position input is not in the expected format.");
            RuleFor(x => x.RoverDirections).Must(p => landRoverCommandHelper.ParseDirections(p) != null)
                  .WithMessage("This movement input is not valid.");
        }

    }

}
