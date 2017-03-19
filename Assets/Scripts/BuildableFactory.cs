using BattleCruisers.Buildings;
using BattleCruisers.Buildings.Turrets;
using BattleCruisers.Cruisers;
using BattleCruisers.UI;
using BattleCruisers.Units;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers
{
	// FELIX  Rename to BuildableFactory?  Merge building and unit methods
	public class BuildableFactory : MonoBehaviour 
	{
		private UIManager _uiManager;
		private PrefabFetcher _prefabFetcher;

		public IDictionary<UnitCategory, IList<Unit>> Units { private get; set; }

		public void Initialise(UIManager uiManager, PrefabFetcher prefabFetcher)
		{
			_uiManager = uiManager;
			_prefabFetcher = prefabFetcher;
		}

		public Building GetBuildingPrefab(BuildingKey buildingKey, Cruiser parentCruiser, Cruiser enemyCruiser)
		{
			Building building = _prefabFetcher.GetBuildingPrefab(buildingKey);
			building.Initialise(_uiManager, parentCruiser, enemyCruiser, this);
			return building;
		}

		// FELIX  Don't hardcode :P  Use database, prefab has TurretStats id?
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

		public Unit GetUnitPrefab(UnitKey unitKey, Cruiser parentCruiser, Cruiser enemyCruiser)
		{
			Unit unit = _prefabFetcher.GetUnitPrefab(unitKey);
			unit.Initialise(_uiManager, parentCruiser, enemyCruiser, this);
			return unit;
		}

		// FELIX  Don't hardcode :P  Use database, prefab has TurretStats id?
		public ITurretStats GetUnitTurretStats(string unitName)
		{
			switch (unitName)
			{
				case "Attack Boat":
					return new TurretStats(0.5f, 1f, 10f, 3f, ignoreGravity: true);
				default:
					throw new ArgumentException();
			}
		}

		public Unit CreateUnit(Unit unitPrefab)
		{
			return CreateBuildable(unitPrefab);
		}

		private T CreateBuildable<T>(T buildablePrefab) where T : BuildableObject
		{
			T buildable = Instantiate<T>(buildablePrefab);
			buildable.Initialise(buildablePrefab);
			return buildable;
		}
	}
}
