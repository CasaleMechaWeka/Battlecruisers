using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
using UnityEngine;

namespace BattleCruisers.Fetchers
{
    // FELIX  NEXT
    // FELIX  Surely I can use polymorphism for this...
    // Generics?
    // BuildableWrapper<>
    // Buildablekey<>
    public class PrefabFactory : IPrefabFactory
	{
		private readonly PrefabFetcher _prefabFetcher;

		public PrefabFactory(PrefabFetcher prefabFetcher)
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
			BuildingWrapper buildingWrapper = Object.Instantiate(buildingWrapperPrefab);
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
			UnitWrapper unitWrapper = Object.Instantiate(unitWrapperPrefab);
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
            Cruiser cruiser = Object.Instantiate(cruiserPrefab);
			cruiser.StaticInitialise();
			return cruiser;
		}
	}
}
