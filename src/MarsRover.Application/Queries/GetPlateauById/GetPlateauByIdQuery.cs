using MarsRover.Application.Queries.GetPlateauById;
using MediatR;
using System;

namespace MarsRover.Application.Queries
{
    public class GetPlateauByIdQuery : IRequest<PlateauModel>
    {
        public Guid PlateauId { get; set; }
        public GetPlateauByIdQuery(Guid plateauId)
        {
            this.PlateauId = plateauId;
        }
    }
}
