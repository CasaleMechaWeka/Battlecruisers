using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Effects.Deaths;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.Utils.Timers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Fetchers
{
    public class PrefabFactory : IPrefabFactory
	{
		private readonly IPrefabCache _prefabCache;
        private readonly IRandomGenerator _randomGenerator;

		public PrefabFactory(IPrefabCache prefabCache)
		{
            Assert.IsNotNull(prefabCache);

			_prefabCache = prefabCache;
            _randomGenerator = RandomGenerator.Instance;
        }

        public IBuildableWrapper<IBuilding> GetBuildingWrapperPrefab(IPrefabKey buildingKey)
		{
            return _prefabCache.GetBuilding(buildingKey);
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
            return _prefabCache.GetUnit(unitKey);
		}

        public IUnit CreateUnit(
            IBuildableWrapper<IUnit> unitWrapperPrefab,
            IUIManager uiManager,
            IFactoryProvider factoryProvider)
		{
            return CreateBuildable(unitWrapperPrefab.UnityObject, uiManager, factoryProvider);
		}

		private TBuildable CreateBuildable<TBuildable>(
            BuildableWrapper<TBuildable> buildableWrapperPrefab,
            IUIManager uiManager,
            IFactoryProvider factoryProvider) where TBuildable : class, IBuildable
		{
            Helper.AssertIsNotNull(buildableWrapperPrefab, uiManager, factoryProvider);

			BuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
			buildableWrapper.gameObject.SetActive(true);
			buildableWrapper.StaticInitialise();
            buildableWrapper.Buildable.Initialise(uiManager, factoryProvider);

            Logging.Log(Tags.PREFAB_FACTORY, $"Building: {buildableWrapper.Buildable}  Prefab id: {buildableWrapperPrefab.GetInstanceID()}  New instance id: {buildableWrapper.GetInstanceID()}");
            return buildableWrapper.Buildable;
		}

        public Cruiser GetCruiserPrefab(IPrefabKey hullKey)
        {
            return _prefabCache.GetCruiser(hullKey);
        }

        public Cruiser CreateCruiser(Cruiser cruiserPrefab)
        {
            Cruiser cruiser = Object.Instantiate(cruiserPrefab);
            cruiser.StaticInitialise();
            return cruiser;
        }

        public CountdownController CreateDeleteCountdown(Transform parent)
        {
            CountdownController newCountdown = Object.Instantiate(_prefabCache.Countdown);
            newCountdown.transform.SetParent(parent, worldPositionStays: false);
            newCountdown.StaticInitialise();
            return newCountdown;
        }

        public IExplosion CreateExplosion(ExplosionKey explosionKey)
        {
            ExplosionController explosionPrefab = _prefabCache.GetExplosion(explosionKey);
            ExplosionController newExplosion = Object.Instantiate(explosionPrefab);
            return newExplosion.Initialise();
        }

        public IShipDeath CreateShipDeath(ShipDeathKey shipDeathKey)
        {
            ShipDeathInitialiser shipDeathPrefab = _prefabCache.GetShipDeath(shipDeathKey);
            ShipDeathInitialiser newShipDeath = Object.Instantiate(shipDeathPrefab);
            return newShipDeath.CreateShipDeath();
        }

        public TProjectile CreateProjectile<TProjectile, TActiavtionArgs, TStats>(ProjectileKey prefabKey, IFactoryProvider factoryProvider)
            where TProjectile : ProjectileControllerBase<TActiavtionArgs, TStats>
            where TActiavtionArgs : ProjectileActivationArgs<TStats>
            where TStats : IProjectileStats
        {
            Assert.IsNotNull(factoryProvider);

            TProjectile prefab = _prefabCache.GetProjectile<TProjectile>(prefabKey);
            TProjectile projectile = Object.Instantiate(prefab);
            projectile.Initialise(factoryProvider);
            return projectile;
        }

        public IDroneController CreateDrone()
        {
            DroneController newDrone = Object.Instantiate(_prefabCache.Drone);
            newDrone.StaticInitialise();
            return newDrone;
        }
    }
}
