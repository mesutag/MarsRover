using MarsRover.Application.Model;
using MediatR;
using System;

namespace MarsRover.Application.Commands.ExplorePlateau
{
    public class ExplorePlateauCommand : IRequest<CommandResponse>
    {
        public Guid PlateauId { get; internal set; }
        public Guid RoverId { get; set; }

        public ExplorePlateauCommand(Guid plateauId, Guid roverId)
        {
            PlateauId = plateauId;
            RoverId = roverId;
        }
    }
}
