using BattleCruisers.Buildings.Turrets;
using BattleCruisers.Cruisers;
using BattleCruisers.UI;
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
		public ITurretStats GetStatsForTurret(string turretName)
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
			return Instantiate<Building>(buildingPrefab);
		}
	}
}
