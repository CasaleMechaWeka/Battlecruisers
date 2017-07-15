using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Fetchers
{
	// FELIX  Surely I can use polymorphism for this...
	public class PrefabFactory : MonoBehaviour, IPrefabFactory
	{
		private PrefabFetcher _prefabFetcher;

		public void Initialise(PrefabFetcher prefabFetcher)
		{
			_prefabFetcher = prefabFetcher;
		}

		public BuildingWrapper GetBuildingWrapperPrefab(BuildingKey buildingKey)
		{
			BuildingWrapper buildingWrapperPrefab = _prefabFetcher.GetBuildingPrefab(buildingKey);
			buildingWrapperPrefab.Initialise();
			buildingWrapperPrefab.Building.StaticInitialise();
			return buildingWrapperPrefab;
		}

		public Building CreateBuilding(BuildingWrapper buildingWrapperPrefab)
		{
			BuildingWrapper buildingWrapper = Instantiate(buildingWrapperPrefab);
			buildingWrapper.gameObject.SetActive(true);
			buildingWrapper.Initialise();
			buildingWrapper.Building.StaticInitialise();
			return buildingWrapper.Building;
		}

		public UnitWrapper GetUnitWrapperPrefab(UnitKey unitKey)
		{
			UnitWrapper unitWrapperPrefab = _prefabFetcher.GetUnitPrefab(unitKey);
			unitWrapperPrefab.Initialise();
			unitWrapperPrefab.Unit.StaticInitialise();
			return unitWrapperPrefab;
		}

		public Unit CreateUnit(UnitWrapper unitWrapperPrefab)
		{
			UnitWrapper unitWrapper = Instantiate(unitWrapperPrefab);
			unitWrapper.gameObject.SetActive(true);
			unitWrapper.Initialise();
			unitWrapper.Unit.StaticInitialise();
			return unitWrapper.Unit;
		}

		public Cruiser GetCruiserPrefab(HullKey hullKey)
		{
			Cruiser cruiser = _prefabFetcher.GetCruiserPrefab(hullKey);
			cruiser.StaticInitialise();
			return cruiser;
		}

		public Cruiser CreateCruiser(Cruiser cruiserPrefab)
		{
			Cruiser cruiser = Instantiate(cruiserPrefab);
			cruiser.StaticInitialise();
			return cruiser;
		}
	}
}
