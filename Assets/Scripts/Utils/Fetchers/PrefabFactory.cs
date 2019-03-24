using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils.Timers;
using UnityEngine;

namespace BattleCruisers.Utils.Fetchers
{
    // PERF  Cache prefabs, so only need to retrieve the first time :)
    public class PrefabFactory : IPrefabFactory
	{
		private readonly PrefabFetcher _prefabFetcher;

		public PrefabFactory(PrefabFetcher prefabFetcher)
		{
			_prefabFetcher = prefabFetcher;
		}

        public IBuildableWrapper<IBuilding> GetBuildingWrapperPrefab(IPrefabKey buildingKey)
		{
            return GetBuildableWrapperPrefab<IBuilding>(buildingKey);
		}

		public IBuilding CreateBuilding(IBuildableWrapper<IBuilding> buildingWrapperPrefab)
		{
            return CreateBuildable(buildingWrapperPrefab.UnityObject);
		}

        public IBuildableWrapper<IUnit> GetUnitWrapperPrefab(IPrefabKey unitKey)
		{
            return GetBuildableWrapperPrefab<IUnit>(unitKey);
		}

        public IUnit CreateUnit(IBuildableWrapper<IUnit> unitWrapperPrefab)
		{
            return CreateBuildable(unitWrapperPrefab.UnityObject);
		}

		private BuildableWrapper<TBuildable> GetBuildableWrapperPrefab<TBuildable>(IPrefabKey buildableKey) where TBuildable : class, IBuildable
		{
			BuildableWrapper<TBuildable> buildableWrapperPrefab = _prefabFetcher.GetPrefab<BuildableWrapper<TBuildable>>(buildableKey);
			buildableWrapperPrefab.Initialise();
			return buildableWrapperPrefab;
		}

		private TBuildable CreateBuildable<TBuildable>(BuildableWrapper<TBuildable> buildableWrapperPrefab) where TBuildable : class, IBuildable
		{
			BuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
			buildableWrapper.gameObject.SetActive(true);
			buildableWrapper.Initialise();
			return buildableWrapper.Buildable;
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

        public CountdownController CreateDeleteCountdown(Transform parent)
        {
            CountdownController countdownPrefab = _prefabFetcher.GetPrefab<CountdownController>(StaticPrefabKeys.UI.DeleteCountdown);
            CountdownController newCountdown = Object.Instantiate(countdownPrefab);
            newCountdown.transform.SetParent(parent, worldPositionStays: false);
            newCountdown.Initialise();
            return newCountdown;
        }

        public CartoonExplosion CreateCartoonExplosion(IExplosionStats explosionStats)
        {
            IPrefabKey explosionKey = GetExplosionKey(explosionStats.Size);
            CartoonExplosion explosionPrefab = _prefabFetcher.GetPrefab<CartoonExplosion>(explosionKey);
            CartoonExplosion newExplosion = Object.Instantiate(explosionPrefab);
            newExplosion.Initialise(explosionStats.ShowTrails);
            return newExplosion;
        }

        private IPrefabKey GetExplosionKey(ExplosionSize explosionSize)
        {
            switch (explosionSize)
            {
                case ExplosionSize.Small:
                    return StaticPrefabKeys.Explosions.CartoonExplosion75;

                case ExplosionSize.Medium:
                    return StaticPrefabKeys.Explosions.CartoonExplosion100;

                case ExplosionSize.Large:
                    return StaticPrefabKeys.Explosions.CartoonExplosion150;

                case ExplosionSize.Giant:
                    return StaticPrefabKeys.Explosions.CartoonExplosion500;

                default:
                    throw new System.ArgumentException();
            }
        }
    }
}
