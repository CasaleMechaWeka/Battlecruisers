using BattleCruisers.Drones;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes.Mock
{
	public class DroneConsumer : IDroneConsumer
	{
		public event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;
		public event EventHandler<DroneStateChangedEventArgs> DroneStateChanged;

		public int NumOfDrones { get; set; }
		public int NumOfDronesRequired { get; set; }
		public DroneConsumerState State { get; set; }
	}
}
