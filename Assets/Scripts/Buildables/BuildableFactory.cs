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
	public interface IBuildableFactory
	{
		BuildingWrapper GetBuildingPrefab(BuildingKey buildingKey);
		Building CreateBuilding(BuildingWrapper buildingPrefab);
		UnitWrapper GetUnitPrefab(UnitKey unitKey);
		Unit CreateUnit(UnitWrapper unitPrefab);
	}

	public class BuildableFactory : MonoBehaviour, IBuildableFactory
	{
		private PrefabFetcher _prefabFetcher;

		public void Initialise(PrefabFetcher prefabFetcher)
		{
			_prefabFetcher = prefabFetcher;
		}

		public BuildingWrapper GetBuildingPrefab(BuildingKey buildingKey)
		{
			BuildingWrapper buildingPrefab = _prefabFetcher.GetBuildingPrefab(buildingKey);

			// Awake() is synonymous to the prefabs constructor.  When the prefab is loaded,
			// Awake is called.  Because this prefab will never be loaded (only copies of it
			// made, and those copies will be loaded), need to explicitly call Awake().
			buildingPrefab.building.Awake();

			return buildingPrefab;
		}

		public Building CreateBuilding(BuildingWrapper buildingPrefab)
		{
			return Instantiate(buildingPrefab).building;
		}

		public UnitWrapper GetUnitPrefab(UnitKey unitKey)
		{
			UnitWrapper unitPrefab = _prefabFetcher.GetUnitPrefab(unitKey);

			// Awake() is synonymous to the prefabs constructor.  When the prefab is loaded,
			// Awake is called.  Because this prefab will never be loaded (only copies of it
			// made, and those copies will be loaded), need to explicitly call Awake().
			unitPrefab.unit.Awake();

			return unitPrefab;
		}

		public Unit CreateUnit(UnitWrapper unitPrefab)
		{
			return Instantiate(unitPrefab).unit;
		}
	}
}
