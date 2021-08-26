using MarsRover.Application.Model;
using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.Application.Commands.LandRover
{
    public class LandRoverCommandHandler : IRequestHandler<LandRoverCommand, CommandResponse>
    {
        private readonly IPlateauRepository plateauRepository;
        private readonly ILandRoverCommandHelper landRoverCommandHelper;
        public LandRoverCommandHandler(IPlateauRepository plateauRepository, ILandRoverCommandHelper landRoverCommandHelper)
        {
            this.plateauRepository = plateauRepository;
            this.landRoverCommandHelper = landRoverCommandHelper;
        }
        public async Task<CommandResponse> Handle(LandRoverCommand request, CancellationToken cancellationToken)
        {
            RoverPosition position = landRoverCommandHelper.ParsePosition(request.RoverPosition);
            List<MovementDirection> directions = landRoverCommandHelper.ParseDirections(request.RoverDirections);

            Plateau plateau = await plateauRepository.FindAsync(request.PlateauId);

            if (plateau == null)
                throw new CustomApplicationException("The Plateau is not found.");


            plateau.LandRover(position, directions);
            plateauRepository.UpdatePlateau(plateau);
            await plateauRepository.UnitOfWork.SaveChangesAsync(default);

            return new CommandResponse(plateau);
        }



    }
}
