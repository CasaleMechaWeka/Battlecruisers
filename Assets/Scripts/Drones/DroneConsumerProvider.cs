using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Drones
{
	public interface IDroneConsumerProvider
	{
		IDroneConsumer RequestDroneConsumer(int numOfDronesRequired);
		void ReleaseDroneConsumer(IDroneConsumer droneConsumer);
	}

	public class DroneConsumerProvider : IDroneConsumerProvider
	{
		private IDroneManager _droneManager;

		public DroneConsumerProvider(IDroneManager droneManager)
		{
			_droneManager = droneManager;
		}

		public IDroneConsumer RequestDroneConsumer(int numOfDronesRequired)
		{
			IDroneConsumer droneConsumer = new DroneConsumer(numOfDronesRequired);
			Assert.IsTrue(_droneManager.CanSupportDroneConsumer(droneConsumer));
			_droneManager.AddDroneConsumer(droneConsumer);
			return droneConsumer;
		}

		public void ReleaseDroneConsumer(IDroneConsumer droneConsumer)
		{
			_droneManager.RemoveDroneConsumer(droneConsumer);
		}
	}
}
