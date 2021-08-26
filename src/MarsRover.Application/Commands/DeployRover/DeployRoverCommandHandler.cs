using MarsRover.Application.Model;
using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MarsRover.Core.AggregateRoots.PlateauAggregate.ValueObjects;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.Application.Commands.DeployRover
{
    public class DeployRoverCommandHandler : IRequestHandler<DeployRoverCommand, CommandResponse>
    {
        private readonly IPlateauRepository plateauRepository;
        private readonly IDeployRoverCommandHelper deployRoverCommandHelper;
        public DeployRoverCommandHandler(IPlateauRepository plateauRepository, IDeployRoverCommandHelper deployRoverCommandHelper)
        {
            this.plateauRepository = plateauRepository;
            this.deployRoverCommandHelper = deployRoverCommandHelper;
        }
        public async Task<CommandResponse> Handle(DeployRoverCommand request, CancellationToken cancellationToken)
        {
            RoverPosition position = deployRoverCommandHelper.ParsePosition(request.RoverPosition);
            List<MovementDirection> directions = deployRoverCommandHelper.ParseDirections(request.RoverDirections);

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
