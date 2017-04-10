using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.UI;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables
{
	public class BuildableFactory : MonoBehaviour 
	{
		private PrefabFetcher _prefabFetcher;

		public void Initialise(PrefabFetcher prefabFetcher)
		{
			_prefabFetcher = prefabFetcher;
		}

		public Building GetBuildingPrefab(BuildingKey buildingKey)
		{
			Building buildingPrefab = _prefabFetcher.GetBuildingPrefab(buildingKey);

			// Awake() is synonymous to the prefabs constructor.  When the prefab is loaded,
			// Awake is called.  Because this prefab will never be loaded (only copies of it
			// made, and those copies will be loaded), need to explicitly call Awake().
			buildingPrefab.Awake();

			return buildingPrefab;
		}

		// FELIX  Don't hardcode :P  Use database, prefab has TurretStats id?
		// FELIX  Remove!
		public ITurretStats GetTurretStats(string turretName)
		{
			switch (turretName)
			{
				case "Shooty Turret":
					return new TurretStats(1.5f, 1f, 20f, 30f, ignoreGravity: true);
				case "Artillery":
					return new TurretStats(0.3f, 0.5f, 250f, 30f, ignoreGravity: false);
				default:
					throw new ArgumentException();
			}
		}

		public Building CreateBuilding(Building buildingPrefab)
		{
			return CreateBuildable(buildingPrefab);
		}

		public Unit GetUnitPrefab(UnitKey unitKey)
		{
			return _prefabFetcher.GetUnitPrefab(unitKey);
		}

		// FELIX  Don't hardcode :P  Use database, prefab has TurretStats id?
		public ITurretStats GetUnitTurretStats(string unitName)
		{
			switch (unitName)
			{
				case "Attack Boat":
				case "Attack Boat 2":
					return new TurretStats(0.5f, 1f, 10f, 3f, ignoreGravity: true);
				default:
					throw new ArgumentException();
			}
		}

		public Unit CreateUnit(Unit unitPrefab, IDroneConsumerProvider droneConsumerProvider)
		{
			// FELIX  Remove unit specific initialisation
			Unit unit = CreateBuildable(unitPrefab);
			unit.SpecificInitialisation(droneConsumerProvider);
			return unit;
		}

		private T CreateBuildable<T>(T buildablePrefab) where T : Buildable
		{
			return Instantiate<T>(buildablePrefab);
		}
	}
}
