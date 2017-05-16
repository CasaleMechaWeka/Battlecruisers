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
		BuildingWrapper GetBuildingWrapperPrefab(BuildingKey buildingKey);
		Building CreateBuilding(BuildingWrapper buildingWrapperPrefab);
		UnitWrapper GetUnitWrapperPrefab(UnitKey unitKey);
		Unit CreateUnit(UnitWrapper unitWrapperPrefab);
	}

	public class BuildableFactory : MonoBehaviour, IBuildableFactory
	{
		private PrefabFetcher _prefabFetcher;

		public void Initialise(PrefabFetcher prefabFetcher)
		{
			_prefabFetcher = prefabFetcher;
		}

		public BuildingWrapper GetBuildingWrapperPrefab(BuildingKey buildingKey)
		{
			BuildingWrapper buildingWrapperPrefab = _prefabFetcher.GetBuildingPrefab(buildingKey);

			// Awake() is synonymous to the prefabs constructor.  When the prefab is loaded,
			// Awake is called.  Because this prefab will never be loaded (only copies of it
			// made, and those copies will be loaded), need to explicitly call Awake().
			buildingWrapperPrefab.building.Awake();

			return buildingWrapperPrefab;
		}

		public Building CreateBuilding(BuildingWrapper buildingWrapperPrefab)
		{
			return Instantiate(buildingWrapperPrefab).building;
		}

		public UnitWrapper GetUnitWrapperPrefab(UnitKey unitKey)
		{
			UnitWrapper unitWrapperPrefab = _prefabFetcher.GetUnitPrefab(unitKey);

			// Awake() is synonymous to the prefabs constructor.  When the prefab is loaded,
			// Awake is called.  Because this prefab will never be loaded (only copies of it
			// made, and those copies will be loaded), need to explicitly call Awake().
			unitWrapperPrefab.unit.Awake();

			return unitWrapperPrefab;
		}

		public Unit CreateUnit(UnitWrapper unitWrapperPrefab)
		{
			return Instantiate(unitWrapperPrefab).unit;
		}
	}
}
