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
	public interface IPrefabFactory
	{
		BuildingWrapper GetBuildingWrapperPrefab(BuildingKey buildingKey);
		Building CreateBuilding(BuildingWrapper buildingWrapperPrefab);

		UnitWrapper GetUnitWrapperPrefab(UnitKey unitKey);
		Unit CreateUnit(UnitWrapper unitWrapperPrefab);

		Cruiser GetCruiserPrefab(HullKey hullKey);
		Cruiser CreateCruiser(Cruiser cruiserPrefab);
	}

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
			buildingWrapperPrefab.FakeAwake();
			return buildingWrapperPrefab;
		}

		public Building CreateBuilding(BuildingWrapper buildingWrapperPrefab)
		{
			return Instantiate(buildingWrapperPrefab).Building;
		}

		public UnitWrapper GetUnitWrapperPrefab(UnitKey unitKey)
		{
			UnitWrapper unitWrapperPrefab = _prefabFetcher.GetUnitPrefab(unitKey);
			unitWrapperPrefab.FakeAwake();
			return unitWrapperPrefab;
		}

		public Unit CreateUnit(UnitWrapper unitWrapperPrefab)
		{
			return Instantiate(unitWrapperPrefab).Unit;
		}

		public Cruiser GetCruiserPrefab(HullKey hullKey)
		{
			return _prefabFetcher.GetCruiserPrefab(hullKey);
		}

		public Cruiser CreateCruiser(Cruiser cruiserPrefab)
		{
			return Instantiate(cruiserPrefab);
		}
	}
}
