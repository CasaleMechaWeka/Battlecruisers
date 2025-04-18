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
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers.Cache;
using UnityEngine;
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
            IUIManager uiManager)
        {
            return CreateBuildable(buildingWrapperPrefab.UnityObject, uiManager);
        }

        public static IBuildableWrapper<IUnit> GetUnitWrapperPrefab(IPrefabKey unitKey)
        {
            return PrefabCache.GetUnit(unitKey);
        }

        public static IUnit CreateUnit(
            IBuildableWrapper<IUnit> unitWrapperPrefab,
            IUIManager uiManager)
        {
            return CreateBuildable(unitWrapperPrefab.UnityObject, uiManager);
        }

        private static TBuildable CreateBuildable<TBuildable>(
            BuildableWrapper<TBuildable> buildableWrapperPrefab,
            IUIManager uiManager) where TBuildable : class, IBuildable
        {
            Helper.AssertIsNotNull(buildableWrapperPrefab, uiManager);

            BuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
            buildableWrapper.gameObject.SetActive(true);
            buildableWrapper.StaticInitialise();
            buildableWrapper.Buildable.Initialise(uiManager);

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

        public static TProjectile CreateProjectile<TProjectile, TActiavtionArgs, TStats>(ProjectileKey prefabKey)
            where TProjectile : ProjectileControllerBase<TActiavtionArgs, TStats>
            where TActiavtionArgs : ProjectileActivationArgs<TStats>
            where TStats : IProjectileStats
        {

            Prefab prefab = PrefabCache.GetProjectile(prefabKey);
            TProjectile projectile = (TProjectile)Object.Instantiate(prefab);
            projectile.Initialise();
            return projectile;
        }

        public static IDroneController CreateDrone()
        {
            DroneController newDrone = Object.Instantiate(PrefabCache.Drone);
            newDrone.StaticInitialise();
            return newDrone;
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
