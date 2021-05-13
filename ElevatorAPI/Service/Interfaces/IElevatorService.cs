using ElevatorAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElevatorAPI.Service.Interfaces
{
    public interface IElevatorService
    {
        ElevatorResult FloorPress(int floor);
        ElevatorResult GetStatus();
        ElevatorResult GetNextFloor();
    }
}
