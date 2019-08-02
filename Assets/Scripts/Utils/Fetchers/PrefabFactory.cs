using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils.Timers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Fetchers
{
    // PERF  Cache prefabs, so only need to retrieve the first time :)
    public class PrefabFactory : IPrefabFactory
	{
		private readonly PrefabFetcher _prefabFetcher;
        private readonly IRandomGenerator _randomGenerator;

		public PrefabFactory(PrefabFetcher prefabFetcher)
		{
            Assert.IsNotNull(prefabFetcher);

			_prefabFetcher = prefabFetcher;
            _randomGenerator = new RandomGenerator();
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

        public IExplosion CreateExplosion(IExplosionStats explosionStats)
        {
            IPrefabKey explosionKey = GetExplosionKey(explosionStats.Size);
            AdvancedExplosion explosionPrefab = _prefabFetcher.GetPrefab<AdvancedExplosion>(explosionKey);
            AdvancedExplosion newExplosion = Object.Instantiate(explosionPrefab);
            newExplosion.Initialise(_randomGenerator);
            return newExplosion;
        }

        private IPrefabKey GetExplosionKey(ExplosionSize explosionSize)
        {
            switch (explosionSize)
            {
                case ExplosionSize.Small:
                    return StaticPrefabKeys.Explosions.HDExplosion75;

                case ExplosionSize.Medium:
                    return StaticPrefabKeys.Explosions.HDExplosion100;

                case ExplosionSize.Large:
                    return StaticPrefabKeys.Explosions.HDExplosion150;

                case ExplosionSize.Giant:
                    return StaticPrefabKeys.Explosions.HDExplosion500;

                default:
                    throw new System.ArgumentException();
            }
        }
    }
}
