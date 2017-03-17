using BattleCruisers.Buildings.Turrets;
using BattleCruisers.Cruisers;
using BattleCruisers.UI;
using BattleCruisers.Units;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildings
{
	public class BuildingFactory : MonoBehaviour 
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

		// FELIX  Factory should nto really store these either?
		public IList<Unit> GetFactoryUnits(string factoryName)
		{
			IList<Unit> units = new List<Unit>();

			switch (factoryName)
			{
				case "Naval Factory":
					return Units[UnitCategory.Naval];
					break;
				default:
					throw new ArgumentException();
			}

			return units;
		}

		public Building CreateBuilding(Building buildingPrefab)
		{
			Building building = Instantiate<Building>(buildingPrefab);
			building.Initialise(buildingPrefab);
			return building;
		}
	}
}
