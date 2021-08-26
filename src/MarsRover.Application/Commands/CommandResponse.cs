using MarsRover.Core.SeedWork;
using System;

namespace MarsRover.Application.Model
{
    public class CommandResponse
    {
        public Guid Id { get; set; }
        public CommandResponse(Entity entity)
        {
            Id = entity.Id;
        }
    }
}
