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
        static Stack<IDroneController> dronePool;
        static Stack<IPoolable<Vector3>>[] shipDeathPool;

        public static void CreatePools()
        {
            dronePool = new Stack<IDroneController>();

            Assert.IsTrue(explosionPoolTargets.Length == StaticPrefabKeys.Explosions.AllKeys.Count);
            explosionPool = new Stack<IPoolable<Vector3>>[explosionPoolTargets.Length];
            shipDeathPool = new Stack<IPoolable<Vector3>>[StaticPrefabKeys.ShipDeaths.AllKeys.Count];

            for (int i = 0; i < explosionPoolTargets.Length; i++)
            {
                explosionPool[i] = new Stack<IPoolable<Vector3>>();
                for (int j = 0; j < explosionPoolTargets[i]; j++)
                    explosionPool[i].Push(CreateExplosion((ExplosionType)i));
            }

            for (int i = 0; i < shipDeathPool.Length; i++)
            {
                shipDeathPool[i] = new Stack<IPoolable<Vector3>>();
                shipDeathPool[i].Push(CreateShipDeath((ShipDeathType)i));
            }
        }

        public static void ClearPool()
        {
            dronePool = null;
            explosionPool = null;
            shipDeathPool = null;
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

            IPoolable<Vector3> explosion = newExplosion.Initialise();
            explosion.Deactivated += (object sender, EventArgs e) => { explosionPool[(int)explosionType].Push(explosion); };
            return explosion;
        }

        public static IPoolable<Vector3> ShowExplosion(ExplosionType explosionType, Vector3 position)
        {
            IPoolable<Vector3> explosion;
            if (explosionPool != null && explosionPool[(int)explosionType].Count > 0)
                explosion = explosionPool[(int)explosionType].Pop();
            else
                explosion = CreateExplosion(explosionType);

            explosion.Activate(position);
            return explosion;
        }

        public static IPoolable<Vector3> CreateShipDeath(ShipDeathType deathType)
        {
            ShipDeathInitialiser shipDeathPrefab = PrefabCache.GetShipDeath(StaticPrefabKeys.ShipDeaths.GetKey(deathType));
            ShipDeathInitialiser newShipDeath = Object.Instantiate(shipDeathPrefab);

            IPoolable<Vector3> shipDeath = newShipDeath.CreateShipDeath();
            shipDeath.Deactivated += (object sender, EventArgs e) => { Debug.Log("PUSH"); shipDeathPool[(int)deathType].Push(shipDeath); };

            return shipDeath;
        }

        public static IPoolable<Vector3> ShowShipDeath(ShipDeathType deathType, Vector3 position, Faction faction)
        {
            IPoolable<Vector3> shipDeath;

            if (shipDeathPool != null && shipDeathPool[(int)deathType].Count > 0)
                shipDeath = shipDeathPool[(int)deathType].Pop();
            else
                shipDeath = CreateShipDeath(deathType);

            shipDeath.Activate(position, faction);
            return shipDeath;
        }

        public static TProjectile CreateProjectile<TProjectile, TActiavtionArgs>(ProjectileKey prefabKey)
            where TProjectile : ProjectileControllerBase<TActiavtionArgs>
            where TActiavtionArgs : ProjectileActivationArgs
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
            newDrone.Deactivated += (object sender, EventArgs e) => { dronePool.Push(newDrone); };

            return newDrone;
        }

        public static IDroneController GetDrone()
        {
            IDroneController drone;
            if (dronePool != null && dronePool.Count > 0)
                drone = dronePool.Pop();
            else
                drone = CreateDrone();

            return drone;
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
