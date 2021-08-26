using FluentValidation;

namespace MarsRover.Application.Commands.CreatePlateau
{
    public class CreatePlateauCommandValidator : AbstractValidator<CreatePlateauCommand>
    {
        public CreatePlateauCommandValidator()
        {
            this.RuleFor(x => x.PlateauSize).NotEmpty()
                   .WithMessage("Please enter the plateau size!");
            this.RuleFor(x => x.PlateauSize).Must(p => CreatePlateauCommandHelper.ParsePlateauSize(p) != null)
                   .WithMessage("The input of plateau size is not in the expected format.");
        }
      
    }

}
