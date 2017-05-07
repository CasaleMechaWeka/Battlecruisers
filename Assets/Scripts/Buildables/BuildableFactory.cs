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
		Building GetBuildingPrefab(BuildingKey buildingKey);
		Building CreateBuilding(Building buildingPrefab);
		Unit GetUnitPrefab(UnitKey unitKey);
		Unit CreateUnit(Unit unitPrefab);
	}

	public class BuildableFactory : MonoBehaviour, IBuildableFactory
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

		public Building CreateBuilding(Building buildingPrefab)
		{
			return CreateBuildable(buildingPrefab);
		}

		public Unit GetUnitPrefab(UnitKey unitKey)
		{
			Unit unitPrefab = _prefabFetcher.GetUnitPrefab(unitKey);
			unitPrefab.Awake();
			return unitPrefab;
		}

		public Unit CreateUnit(Unit unitPrefab)
		{
			return CreateBuildable(unitPrefab);
		}

		private T CreateBuildable<T>(T buildablePrefab) where T : Buildable
		{
			return Instantiate<T>(buildablePrefab);
		}
	}
}
