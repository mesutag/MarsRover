using MarsRover.Application.Model;
using MediatR;
using System;

namespace MarsRover.Application.Commands.StartRover
{
    public class StartRoverMissionCommand : IRequest<CommandResponse>
    {
        public Guid PlateauId { get; internal set; }
        public Guid RoverId { get; set; }

        public StartRoverMissionCommand(Guid plateauId, Guid roverId)
        {
            PlateauId = plateauId;
            RoverId = roverId;
        }
    }
}
