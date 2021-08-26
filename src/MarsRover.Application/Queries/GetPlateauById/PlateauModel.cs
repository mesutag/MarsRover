using System;
using System.Collections.Generic;

namespace MarsRover.Application.Queries.GetPlateauById
{
    public class PlateauModel
    {
        public Guid PlateauId { get; set; }
        public List<RoverModel> Rovers { get; set; }
    }
}