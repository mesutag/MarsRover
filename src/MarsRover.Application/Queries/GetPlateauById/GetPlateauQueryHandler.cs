using MarsRover.Application.Queries.GetPlateauById;
using MarsRover.Core.AggregateRoots.PlateauAggregate;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.Application.Queries
{
    public class GetPlateauQueryHandler : IRequestHandler<GetPlateauByIdQuery, PlateauModel>
    {
        private readonly IPlateauRepository plateauRepository;
        public GetPlateauQueryHandler(IPlateauRepository plateauRepository)
        {
            this.plateauRepository = plateauRepository;
        }
        public async Task<PlateauModel> Handle(GetPlateauByIdQuery request, CancellationToken cancellationToken)
        {
            Plateau plateau = await plateauRepository.FindAsync(request.PlateauId);
            return new PlateauModel
            {
                PlateauId = plateau.Id,
                Rovers = plateau.LandedRovers.Select(rover => new RoverModel
                {
                    RoverId = rover.Id,
                    RoverPosition = new RoverPositionModel
                    {
                        X = rover.Position.X,
                        Y = rover.Position.Y,
                        Direction = rover.Position.Direction
                    }
                }).ToList()
            };
        }
    }
}
