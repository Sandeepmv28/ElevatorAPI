using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElevatorAPI.Models
{
    public class ElevatorResult
    {
        public string Status { get; set; }
        public int CurrentFloor { get; set; }
        public string ElevatorMessage { get; set; }
    }
}
