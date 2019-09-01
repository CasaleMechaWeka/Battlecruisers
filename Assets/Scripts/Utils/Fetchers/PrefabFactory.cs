using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Timers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Fetchers
{
    // PERF  Cache prefabs, so only need to retrieve the first time :)  Hm, maybe Unity already does this?
    public class PrefabFactory : IPrefabFactory
	{
		private readonly PrefabFetcher _prefabFetcher;
        private readonly IRandomGenerator _randomGenerator;

		public PrefabFactory(PrefabFetcher prefabFetcher)
		{
            Assert.IsNotNull(prefabFetcher);

			_prefabFetcher = prefabFetcher;
            _randomGenerator = RandomGenerator.Instance;
        }

        public IBuildableWrapper<IBuilding> GetBuildingWrapperPrefab(IPrefabKey buildingKey)
		{
            return GetBuildableWrapperPrefab<IBuilding>(buildingKey);
		}

		public IBuilding CreateBuilding(
            IBuildableWrapper<IBuilding> buildingWrapperPrefab,
            IUIManager uiManager,
            IFactoryProvider factoryProvider)
		{
            return CreateBuildable(buildingWrapperPrefab.UnityObject, uiManager, factoryProvider);
		}

        public IBuildableWrapper<IUnit> GetUnitWrapperPrefab(IPrefabKey unitKey)
		{
            return GetBuildableWrapperPrefab<IUnit>(unitKey);
		}

        public IUnit CreateUnit(
            IBuildableWrapper<IUnit> unitWrapperPrefab,
            IUIManager uiManager,
            IFactoryProvider factoryProvider)
		{
            return CreateBuildable(unitWrapperPrefab.UnityObject, uiManager, factoryProvider);
		}

		private BuildableWrapper<TBuildable> GetBuildableWrapperPrefab<TBuildable>(IPrefabKey buildableKey) where TBuildable : class, IBuildable
		{
			BuildableWrapper<TBuildable> buildableWrapperPrefab = _prefabFetcher.GetPrefab<BuildableWrapper<TBuildable>>(buildableKey);
			buildableWrapperPrefab.Initialise();
			return buildableWrapperPrefab;
		}

		private TBuildable CreateBuildable<TBuildable>(
            BuildableWrapper<TBuildable> buildableWrapperPrefab,
            IUIManager uiManager,
            IFactoryProvider factoryProvider) where TBuildable : class, IBuildable
		{
            Helper.AssertIsNotNull(buildableWrapperPrefab, uiManager, factoryProvider);

			BuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
			buildableWrapper.gameObject.SetActive(true);
			buildableWrapper.Initialise();
            buildableWrapper.Buildable.Initialise(uiManager, factoryProvider);
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

        public IExplosion CreateBulletImpactExplosion()
        {
            BulletImpactExplosion explosionPrefab = _prefabFetcher.GetPrefab<BulletImpactExplosion>(StaticPrefabKeys.Explosions.BulletImpact);
            BulletImpactExplosion newExplosion = Object.Instantiate(explosionPrefab);
            newExplosion.Initialise();
            return newExplosion;
        }

        public IExplosion CreateAdvancedExplosion(ExplosionKey explosionKey)
        {
            AdvancedExplosion explosionPrefab = _prefabFetcher.GetPrefab<AdvancedExplosion>(explosionKey);
            AdvancedExplosion newExplosion = Object.Instantiate(explosionPrefab);
            newExplosion.Initialise(_randomGenerator);
            return newExplosion;
        }

        public TProjectile CreateProjectile<TProjectile, TActiavtionArgs, TStats>(ProjectileKey prefabKey, IFactoryProvider factoryProvider)
            where TProjectile : ProjectileControllerBase<TActiavtionArgs, TStats>
            where TActiavtionArgs : ProjectileActivationArgs<TStats>
            where TStats : IProjectileStats
        {
            Assert.IsNotNull(factoryProvider);

            TProjectile prefab = _prefabFetcher.GetPrefab<TProjectile>(prefabKey);
            TProjectile projectile = Object.Instantiate(prefab);
            projectile.Initialise(factoryProvider);
            return projectile;
        }
    }
}
