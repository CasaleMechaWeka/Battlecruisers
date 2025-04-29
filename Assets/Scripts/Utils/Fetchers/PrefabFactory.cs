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
        static int[] projectilePoolTargets = new int[20] { 20, 20, 50, 20, 5, 5, 5, 5, 5, 20, 20, 10, 10, 16, 10, 10, 10, 10, 6, 6 };
        static Stack<IPoolable<Vector3>>[] explosionPool;
        static Stack<ProjectileControllerBase>[] projectilePool;
        static Stack<IDroneController> dronePool;
        static Stack<IPoolable<Vector3>>[] shipDeathPool;

        public static void CreatePools()
        {
            Assert.IsTrue(explosionPoolTargets.Length == StaticPrefabKeys.Explosions.AllKeys.Count);
            Assert.IsTrue(projectilePoolTargets.Length == StaticPrefabKeys.Projectiles.AllKeys.Count);

            dronePool = new Stack<IDroneController>();

            explosionPool = new Stack<IPoolable<Vector3>>[explosionPoolTargets.Length];
            projectilePool = new Stack<ProjectileControllerBase>[projectilePoolTargets.Length];
            shipDeathPool = new Stack<IPoolable<Vector3>>[StaticPrefabKeys.ShipDeaths.AllKeys.Count];

            for (int i = 0; i < explosionPoolTargets.Length; i++)
            {
                explosionPool[i] = new Stack<IPoolable<Vector3>>();
                for (int j = 0; j < explosionPoolTargets[i]; j++)
                    explosionPool[i].Push(CreateExplosion((ExplosionType)i));
            }

            for (int i = 0; i < projectilePoolTargets.Length; i++)
            {
                projectilePool[i] = new Stack<ProjectileControllerBase>();
                ProjectileControllerType controllerType = StaticPrefabKeys.Projectiles.GetProjectileControllerType((ProjectileType)i);

                for (int j = 0; j < projectilePoolTargets[i]; j++)
                    switch (controllerType)
                    {
                        case ProjectileControllerType.ProjectileController:
                            projectilePool[i].Push(CreateProjectile<ProjectileController>((ProjectileType)i));
                            break;
                        case ProjectileControllerType.BombController:
                            projectilePool[i].Push(CreateProjectile<BombController>((ProjectileType)i));
                            break;
                        case ProjectileControllerType.RocketController:
                            projectilePool[i].Push(CreateProjectile<RocketController>((ProjectileType)i));
                            break;
                        case ProjectileControllerType.MissileController:
                            projectilePool[i].Push(CreateProjectile<MissileController>((ProjectileType)i));
                            break;
                        default: throw new ArgumentException();
                    }
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
            projectilePool = null;
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
            shipDeath.Deactivated += (object sender, EventArgs e) => { shipDeathPool[(int)deathType].Push(shipDeath); };

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

        public static TProjectile CreateProjectile<TProjectile>(ProjectileType projectileType)
            where TProjectile : ProjectileControllerBase
        {
            Prefab prefab = PrefabCache.GetProjectile(StaticPrefabKeys.Projectiles.GetKey(projectileType));
            TProjectile projectile = (TProjectile)Object.Instantiate(prefab);
            projectile.Initialise();
            projectile.Deactivated += (object sender, EventArgs e) => { projectilePool[(int)projectileType].Push(projectile); };

            return projectile;
        }

        public static TProjectile GetProjectile<TProjectile>(ProjectileType projectileType, ProjectileActivationArgs activationArgs)
            where TProjectile : ProjectileControllerBase
        {
            TProjectile projectile;

            if (projectilePool[(int)projectileType].Count > 0)
                projectile = (TProjectile)projectilePool[(int)projectileType].Pop();
            else
                projectile = CreateProjectile<TProjectile>(projectileType);

            projectile.Activate(activationArgs);
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
