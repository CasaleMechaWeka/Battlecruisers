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
		private IDroneManager _droneManager;

		public int numOfDronesProvided;

		public override void Initialise(UIManager uiManager, Cruiser parentCruiser, Cruiser enemyCruiser, BuildableFactory buildableFactory, BattleCruisers.Drones.IDroneManager droneManager)
		{
			base.Initialise(uiManager, parentCruiser, enemyCruiser, buildableFactory, droneManager);
			_droneManager = droneManager;
		}

		public override void Initialise(Buildable buildable)
		{
			base.Initialise(buildable);

			DroneStation droneStation = buildable as DroneStation;
			Assert.IsNotNull(droneStation);

			_droneManager = droneStation._droneManager;
		}

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
