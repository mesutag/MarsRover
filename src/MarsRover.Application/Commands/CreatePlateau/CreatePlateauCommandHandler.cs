using MarsRover.Application.Model;
using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.Application.Commands.CreatePlateau
{
    public class CreatePlateauCommandHandler : IRequestHandler<CreatePlateauCommand, CommandResponse>
    {
        private readonly IPlateauRepository plateauRepository;
        public CreatePlateauCommandHandler(IPlateauRepository plateauRepository)
        {
            this.plateauRepository = plateauRepository;
        }
        public async Task<CommandResponse> Handle(CreatePlateauCommand request, CancellationToken cancellationToken)
        {
            Size plateauSize = CreatePlateauCommandHelper.ParsePlateauSize(request.PlateauSize);

            Plateau plateau = new(plateauSize);
            await plateauRepository.CreatePlateauAsync(plateau);
            await plateauRepository.UnitOfWork.SaveChangesAsync(default);
            return new CommandResponse(plateau);
        }
    }
}
