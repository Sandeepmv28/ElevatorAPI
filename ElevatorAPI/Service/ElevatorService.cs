using ElevatorAPI.Common;
using ElevatorAPI.Models;
using ElevatorAPI.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorAPI.Service
{
    public class ElevatorService : IElevatorService
	{
		private bool[] floorReady;
		public static int CurrentFloor = 1;
		public static int CurrentRunningAtFloor = 0;
		private int topfloor;
		public ElevatorStatus Status = ElevatorStatus.STOPPED;
		public ElevatorStatus CurrentRunningStatus = ElevatorStatus.STOPPED;
		public string ElevatorMessage = "";		
		private readonly IConfiguration _configuration;
		private readonly int NumberOfFloors;
		public ElevatorService(IConfiguration configuration)
		{
			NumberOfFloors = configuration.GetValue<int>("NumberOfFloors");
			floorReady = new bool[NumberOfFloors + 1];
			topfloor = configuration.GetValue<int>("NumberOfFloors");
			_configuration = configuration;
		}

		private void Stop(int floor)
		{
			CurrentRunningStatus = Status = ElevatorStatus.STOPPED;
			CurrentRunningAtFloor = CurrentFloor = floor;
			floorReady[floor] = false;
			ElevatorMessage = $"Stopped at floor {floor}";
		}

		private void Descend(int floor)
		{
			CurrentRunningStatus = ElevatorStatus.DOWN;
			for (int i = CurrentFloor; i >= 1; i--)
			{
				//Thread.Sleep(1000); // uncomment it to test the status of elevator.
				CurrentRunningAtFloor = i;
				if (floorReady[i])
					Stop(floor);
				else
					continue;
			}

			Status = ElevatorStatus.STOPPED;
			ElevatorMessage = $"Waiting at floor: {CurrentFloor}";
		}

		private void Ascend(int floor)
		{
			CurrentRunningStatus = ElevatorStatus.UP;
			for (int i = CurrentFloor; i <= topfloor; i++)
			{
				CurrentRunningAtFloor = i;
				//Thread.Sleep(1000); // uncomment it to test the status of elevator.
				if (floorReady[i])
					Stop(floor);
				else
					continue;
			}

			Status = ElevatorStatus.STOPPED;
		}

		void StayPut()
		{
			ElevatorMessage = "That's our current floor";
		}

		public ElevatorResult FloorPress(int floor)
		{
			var result = new ElevatorResult
			{
				Status = Status.ToString(),
				CurrentFloor = CurrentFloor

			};

			if (floor > topfloor)
			{
				result.ElevatorMessage = $"We only have {topfloor} floors";
				return result;
			}

			floorReady[floor] = true;

			switch (Status)
			{

				case ElevatorStatus.DOWN:
					Descend(floor);
					break;

				case ElevatorStatus.STOPPED:
					if (CurrentFloor < floor)
						Ascend(floor);
					else if (CurrentFloor == floor)
						StayPut();
					else
						Descend(floor);
					break;

				case ElevatorStatus.UP:
					Ascend(floor);
					break;

				default:
					break;
			}

			result.Status = Status.ToString();
			result.CurrentFloor = CurrentFloor;
			result.ElevatorMessage = ElevatorMessage;
			return result;
		}
	
		public ElevatorResult GetStatus()
        {
			return new ElevatorResult
			{
				Status = CurrentRunningStatus.ToString(),
				CurrentFloor = CurrentRunningStatus == ElevatorStatus.STOPPED ? CurrentFloor : CurrentRunningAtFloor
			};
        }

		public ElevatorResult GetNextFloor()
		{
		    var result = new ElevatorResult
			{
				Status = CurrentRunningStatus.ToString()
			};

			if (NumberOfFloors < CurrentFloor + 1)
            {
				result.Status = CurrentRunningStatus.ToString();
				result.CurrentFloor = CurrentFloor;
				result.ElevatorMessage = $"You are at top floor {CurrentFloor}";
				return result;
			}

			if(CurrentRunningStatus == ElevatorStatus.STOPPED)
            {
				result = FloorPress(CurrentFloor+1);
			}
			else
            {
				result.Status = CurrentRunningStatus.ToString();
				result.CurrentFloor = CurrentRunningAtFloor;
				result.ElevatorMessage = $"Elevator is moving towards {CurrentRunningStatus}";
			}

			return result;
		}
	}
	
}
