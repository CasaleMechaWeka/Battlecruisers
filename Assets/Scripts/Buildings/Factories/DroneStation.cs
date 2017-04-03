using BattleCruisers.Cruisers;
using BattleCruisers.Buildings.Turrets;
using BattleCruisers.UI;
using BattleCruisers.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.Buildings.Factories
{
	public class DroneStation : Building
	{
		public int numOfDronesProvided;

		protected override void OnBuildingCompleted()
		{
			base.OnBuildingCompleted();

			// Add drones to DroneManager
		}

		protected override void OnDestroyed()
		{
			// Remove drones form DroneManager
		}
	}
}
