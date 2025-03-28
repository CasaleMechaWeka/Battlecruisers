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
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace BattleCruisers.Utils.Fetchers
{
    public static class PrefabFactory
    {
        public static IBuildableWrapper<IBuilding> GetBuildingWrapperPrefab(IPrefabKey buildingKey)
        {
            return PrefabCache.GetBuilding(buildingKey);
        }

        public static IBuilding CreateBuilding(
            IBuildableWrapper<IBuilding> buildingWrapperPrefab,
            IUIManager uiManager,
            FactoryProvider factoryProvider)
        {
            return CreateBuildable(buildingWrapperPrefab.UnityObject, uiManager, factoryProvider);
        }

        public static IBuildableWrapper<IUnit> GetUnitWrapperPrefab(IPrefabKey unitKey)
        {
            return PrefabCache.GetUnit(unitKey);
        }

        public static IUnit CreateUnit(
            IBuildableWrapper<IUnit> unitWrapperPrefab,
            IUIManager uiManager,
            FactoryProvider factoryProvider)
        {
            return CreateBuildable(unitWrapperPrefab.UnityObject, uiManager, factoryProvider);
        }

        private static TBuildable CreateBuildable<TBuildable>(
            BuildableWrapper<TBuildable> buildableWrapperPrefab,
            IUIManager uiManager,
            FactoryProvider factoryProvider) where TBuildable : class, IBuildable
        {
            Helper.AssertIsNotNull(buildableWrapperPrefab, uiManager, factoryProvider);

            BuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
            buildableWrapper.gameObject.SetActive(true);
            buildableWrapper.StaticInitialise();
            buildableWrapper.Buildable.Initialise(uiManager, factoryProvider);

            Logging.Log(Tags.PREFAB_FACTORY, $"Building: {buildableWrapper.Buildable}  Prefab id: {buildableWrapperPrefab.GetInstanceID()}  New instance id: {buildableWrapper.GetInstanceID()}");
            return buildableWrapper.Buildable;
        }

        public static Cruiser GetCruiserPrefab(IPrefabKey hullKey)
        {
            return PrefabCache.GetCruiser(hullKey);
        }

        public static Cruiser CreateCruiser(Cruiser cruiserPrefab)
        {
            Cruiser cruiser = Object.Instantiate(cruiserPrefab);
            cruiser.StaticInitialise();
            return cruiser;
        }

        public static IPoolable<Vector3> CreateExplosion(ExplosionKey explosionKey)
        {
            ExplosionController explosionPrefab = PrefabCache.GetExplosion(explosionKey);
            ExplosionController newExplosion = Object.Instantiate(explosionPrefab);
            return newExplosion.Initialise();
        }

        public static IPoolable<Vector3> CreateShipDeath(ShipDeathKey shipDeathKey)
        {
            ShipDeathInitialiser shipDeathPrefab = PrefabCache.GetShipDeath(shipDeathKey);
            ShipDeathInitialiser newShipDeath = Object.Instantiate(shipDeathPrefab);
            return newShipDeath.CreateShipDeath();
        }

        public static TProjectile CreateProjectile<TProjectile, TActiavtionArgs, TStats>(ProjectileKey prefabKey, FactoryProvider factoryProvider)
            where TProjectile : ProjectileControllerBase<TActiavtionArgs, TStats>
            where TActiavtionArgs : ProjectileActivationArgs<TStats>
            where TStats : IProjectileStats
        {
            Assert.IsNotNull(factoryProvider);

            Prefab prefab = PrefabCache.GetProjectile(prefabKey);
            TProjectile projectile = (TProjectile)Object.Instantiate(prefab);
            projectile.Initialise(factoryProvider);
            return projectile;
        }

        public static IDroneController CreateDrone()
        {
            DroneController newDrone = Object.Instantiate(PrefabCache.Drone);
            newDrone.StaticInitialise();
            return newDrone;
        }

        public static IPoolable<AudioSourceActivationArgs> CreateAudioSource(IDeferrer realTimeDeferrer)
        {
            Assert.IsNotNull(realTimeDeferrer);

            AudioSourceInitialiser audioSourceInitialiser = Object.Instantiate(PrefabCache.AudioSource);
            return audioSourceInitialiser.Initialise(realTimeDeferrer);
        }

        public static CaptainExo GetCaptainExo(IPrefabKey captainExoKey)
        {
            return PrefabCache.GetCaptainExo(captainExoKey);
        }

        public static Bodykit GetBodykit(IPrefabKey bodykitKey)
        {
            return PrefabCache.GetBodykit(bodykitKey);
        }

        public static VariantPrefab GetVariant(IPrefabKey variantKey)
        {
            return PrefabCache.GetVariant(variantKey);
        }
    }
}
