using MarsRover.Application.Model;
using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.Application.Commands.ExplorePlateau
{
    public class ExplorePlateauCommandHandler : IRequestHandler<ExplorePlateauCommand, CommandResponse>
    {
        private readonly IPlateauRepository plateauRepository;
        public ExplorePlateauCommandHandler(IPlateauRepository plateauRepository)
        {
            this.plateauRepository = plateauRepository;
        }
        public async Task<CommandResponse> Handle(ExplorePlateauCommand request, CancellationToken cancellationToken)
        {
            Plateau plateau = await plateauRepository.FindAsync(request.PlateauId);
            plateau.ExplorePlateau(request.RoverId);
            plateauRepository.UpdatePlateau(plateau);
            await plateauRepository.UnitOfWork.SaveChangesAsync(default);

            return new CommandResponse(plateau);
        }
    }
}
