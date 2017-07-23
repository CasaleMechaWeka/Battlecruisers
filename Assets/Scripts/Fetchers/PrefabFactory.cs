using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
using UnityEngine;

namespace BattleCruisers.Fetchers
{
    public class PrefabFactory : IPrefabFactory
	{
		private readonly PrefabFetcher _prefabFetcher;

		public PrefabFactory(PrefabFetcher prefabFetcher)
		{
			_prefabFetcher = prefabFetcher;
		}

        public BuildingWrapper GetBuildingWrapperPrefab(IPrefabKey buildingKey)
		{
            return (BuildingWrapper)GetBuildableWrapperPrefab<Building>(buildingKey);
		}

		public Building CreateBuilding(BuildingWrapper buildingWrapperPrefab)
		{
            return CreateBuildable(buildingWrapperPrefab);
		}

		public UnitWrapper GetUnitWrapperPrefab(IPrefabKey unitKey)
		{
            return (UnitWrapper)GetBuildableWrapperPrefab<Unit>(unitKey);
		}

		public Unit CreateUnit(UnitWrapper unitWrapperPrefab)
		{
            return CreateBuildable(unitWrapperPrefab);
		}

		public Cruiser GetCruiserPrefab(IPrefabKey hullKey)
		{
            Cruiser cruiser = _prefabFetcher.GetPrefab<Cruiser>(hullKey);
			cruiser.StaticInitialise();
			return cruiser;
		}

		public Cruiser CreateCruiser(Cruiser cruiserPrefab)
		{
            Cruiser cruiser = Object.Instantiate(cruiserPrefab);
			cruiser.StaticInitialise();
			return cruiser;
		}

		private BuildableWrapper<TBuildable> GetBuildableWrapperPrefab<TBuildable>(IPrefabKey buildableKey) where TBuildable : Buildable
		{
			BuildableWrapper<TBuildable> buildableWrapperPrefab = _prefabFetcher.GetPrefab<BuildableWrapper<TBuildable>>(buildableKey);
			buildableWrapperPrefab.Initialise();
			buildableWrapperPrefab.Buildable.StaticInitialise();
			return buildableWrapperPrefab;
		}

		private TBuildable CreateBuildable<TBuildable>(BuildableWrapper<TBuildable> buildableWrapperPrefab) where TBuildable : Buildable
		{
			BuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
			buildableWrapper.gameObject.SetActive(true);
			buildableWrapper.Initialise();
			buildableWrapper.Buildable.StaticInitialise();
			return buildableWrapper.Buildable;
		}
	}
}
