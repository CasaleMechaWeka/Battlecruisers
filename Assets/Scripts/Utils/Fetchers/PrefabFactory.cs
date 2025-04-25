using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
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
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace BattleCruisers.Utils.Fetchers
{
    public static class PrefabFactory
    {
        static int[] explosionPoolTargets = new int[15] { 5, 5, 50, 10, 5, 10, 10, 5, 5, 10, 2, 4, 10, 1, 10 };
        static Stack<IPoolable<Vector3>>[] explosionPool;

        public static void CreateExplosionPool()
        {
            Assert.IsTrue(explosionPoolTargets.Length == StaticPrefabKeys.Explosions.AllKeys.Count);
            explosionPool = new Stack<IPoolable<Vector3>>[explosionPoolTargets.Length];

            for (int i = 0; i < explosionPoolTargets.Length; i++)
            {
                explosionPool[i] = new Stack<IPoolable<Vector3>>();
                for (int j = 0; j < explosionPoolTargets[i]; j++)
                    explosionPool[i].Push(CreateExplosion((ExplosionType)i));
            }
        }

        public static void ClearPool()
        {
            explosionPool = null;
        }

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

        public static IPoolable<Vector3> CreateExplosion(ExplosionType explosionType)
        {
            ExplosionController explosionPrefab = PrefabCache.GetExplosion(StaticPrefabKeys.Explosions.GetKey(explosionType));
            ExplosionController newExplosion = Object.Instantiate(explosionPrefab);
            return newExplosion.Initialise();
        }

        public static IPoolable<Vector3> ShowExplosion(ExplosionType explosionType, Vector3 position)
        {
            IPoolable<Vector3> explosion;
            if (explosionPool != null && explosionPool[(int)explosionType].Count > 0)
            {
                explosion = explosionPool[(int)explosionType].Pop();
                explosion.Activate(position);
                explosion.Deactivated += (object sender, EventArgs e) => { explosionPool[(int)explosionType].Push(explosion); };
            }
            else
            {
                explosion = CreateExplosion(explosionType);
                explosion.Activate(position);
            }
            return explosion;
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
