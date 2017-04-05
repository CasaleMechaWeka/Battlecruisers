using BattleCruisers.Buildings.Turrets;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.UI;
using BattleCruisers.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Buildings.Factories
{
	public class DroneStation : Building
	{
		public int numOfDronesProvided;

		protected override void OnBuildingCompleted()
		{
			base.OnBuildingCompleted();

			_droneManager.NumOfDrones += numOfDronesProvided;
		}

		protected override void OnDestroyed()
		{
			_droneManager.NumOfDrones -= numOfDronesProvided;
		}
	}
}
