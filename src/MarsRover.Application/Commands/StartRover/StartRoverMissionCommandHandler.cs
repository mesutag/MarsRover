using MarsRover.Application.Model;
using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.Application.Commands.StartRover
{
    public class StartRoverMissionCommandHandler : IRequestHandler<StartRoverMissionCommand, CommandResponse>
    {
        private readonly IPlateauRepository plateauRepository;
        public StartRoverMissionCommandHandler(IPlateauRepository plateauRepository)
        {
            this.plateauRepository = plateauRepository;
        }
        public async Task<CommandResponse> Handle(StartRoverMissionCommand request, CancellationToken cancellationToken)
        {
            Plateau plateau = await plateauRepository.FindAsync(request.PlateauId);
            plateau.StartRover(request.RoverId);
            plateauRepository.UpdatePlateau(plateau);
            await plateauRepository.UnitOfWork.SaveChangesAsync(default);

            return new CommandResponse(plateau);
        }
    }
}
