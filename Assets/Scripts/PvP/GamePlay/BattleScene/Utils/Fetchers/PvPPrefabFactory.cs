using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles;
using BattleCruisers.Projectiles;
using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Data.Models.PrefabKeys;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Object = UnityEngine.Object;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using System.Collections.Generic;
using UnityEngine.Assertions;
using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers
{
    public static class PvPPrefabFactory
    {
        static int[] explosionPoolTargets = new int[15] { 5, 5, 50, 10, 5, 10, 10, 5, 5, 10, 2, 4, 10, 1, 10 };
        static int[] projectilePoolTargets = new int[20] { 20, 20, 50, 20, 5, 5, 5, 5, 5, 20, 20, 10, 10, 16, 10, 10, 10, 10, 6, 6 };
        static Stack<IPoolable<Vector3>>[] explosionPool;
        static Stack<PvPProjectileControllerBase>[] projectilePool;
        static Stack<IDroneController> dronePool;
        static Stack<IPoolable<Vector3>>[] shipDeathPool;

        public static void CreatePools()
        {
            Assert.IsTrue(explosionPoolTargets.Length == PvPStaticPrefabKeys.PvPExplosions.AllKeys.Count);
            Assert.IsTrue(projectilePoolTargets.Length == PvPStaticPrefabKeys.PvPProjectiles.AllKeys.Count);

            dronePool = new Stack<IDroneController>();
            explosionPool = new Stack<IPoolable<Vector3>>[explosionPoolTargets.Length];
            projectilePool = new Stack<PvPProjectileControllerBase>[projectilePoolTargets.Length];
            shipDeathPool = new Stack<IPoolable<Vector3>>[PvPStaticPrefabKeys.PvPShipDeaths.AllKeys.Count];

            for (int i = 0; i < explosionPoolTargets.Length; i++)
            {
                explosionPool[i] = new Stack<IPoolable<Vector3>>();
                for (int j = 0; j < explosionPoolTargets[i]; j++)
                    explosionPool[i].Push(CreateExplosion((PvPExplosionType)i));
            }

            for (int i = 0; i < shipDeathPool.Length; i++)
            {
                shipDeathPool[i] = new Stack<IPoolable<Vector3>>();
                shipDeathPool[i].Push(CreateShipDeath((PvPShipDeathType)i));
            }

            for (int i = 0; i < projectilePoolTargets.Length; i++)
            {
                projectilePool[i] = new Stack<PvPProjectileControllerBase>();
                PvPProjectileControllerType controllerType = PvPStaticPrefabKeys.PvPProjectiles.GetProjectileControllerType((PvPProjectileType)i);

                for (int j = 0; j < projectilePoolTargets[i]; j++)
                    switch (controllerType)
                    {
                        case PvPProjectileControllerType.ProjectileController:
                            projectilePool[i].Push(CreateProjectile<PvPProjectileController>((PvPProjectileType)i));
                            break;
                        case PvPProjectileControllerType.BombController:
                            projectilePool[i].Push(CreateProjectile<PvPBombController>((PvPProjectileType)i));
                            break;
                        case PvPProjectileControllerType.RocketController:
                            projectilePool[i].Push(CreateProjectile<PvPRocketController>((PvPProjectileType)i));
                            break;
                        case PvPProjectileControllerType.MissileController:
                            projectilePool[i].Push(CreateProjectile<PvPMissileController>((PvPProjectileType)i));
                            break;
                        default: throw new ArgumentException();
                    }
            }
        }
        public static void ClearPool()
        {
            dronePool = null;
            explosionPool = null;
            projectilePool = null;
            shipDeathPool = null;
        }


        public static IPvPBuildableWrapper<IPvPBuilding> GetBuildingWrapperPrefab(IPrefabKey buildingKey)
        {
            return PvPPrefabCache.GetBuilding(buildingKey);
        }

        public static IPvPBuilding CreateBuilding(
            IPvPBuildableWrapper<IPvPBuilding> buildingWrapperPrefab,
            ulong clientID)
        {
            return CreateBuildingBuildable(buildingWrapperPrefab.UnityObject, clientID);
        }

        public static IPvPBuildableWrapper<IPvPUnit> GetUnitWrapperPrefab(IPrefabKey unitKey)
        {
            return PvPPrefabCache.GetUnit(unitKey);
        }

        public static PvPBuildableOutlineController GetOutline(IPrefabKey outlineKey)
        {
            return PvPPrefabCache.GetOutline(outlineKey);
        }

        public static IPvPUnit CreateUnit(
            IPvPBuildableWrapper<IPvPUnit> unitWrapperPrefab
            /* IPvPUIManager uiManager , */)
        {
            var _unitBuildable = CreateUnitBuildable(unitWrapperPrefab.UnityObject);
            return _unitBuildable;
        }

        private static TBuildable CreateBuildingBuildable<TBuildable>(
            PvPBuildableWrapper<TBuildable> buildableWrapperPrefab,
            ulong clientID) where TBuildable : class, IPvPBuilding
        {
            PvPHelper.AssertIsNotNull(buildableWrapperPrefab);
            PvPBuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
            buildableWrapper.gameObject.SetActive(true);
            buildableWrapper.GetComponent<NetworkObject>().SpawnWithOwnership(clientID);
            buildableWrapper.GetComponent<NetworkObject>().DontDestroyWithOwner = false;
            buildableWrapper.StaticInitialise();
            buildableWrapper.Buildable.Initialise();
            return buildableWrapper.Buildable;
        }
        public static PvPBuildableOutlineController CreateOutline(PvPBuildableOutlineController outlinePrefab)
        {
            PvPBuildableOutlineController outline = Object.Instantiate(outlinePrefab);
            outline.StaticInitialise();
            return outline;
        }

        private static TBuildable CreateUnitBuildable<TBuildable>(
            PvPBuildableWrapper<TBuildable> buildableWrapperPrefab) where TBuildable : class, IPvPUnit
        {
            PvPHelper.AssertIsNotNull(buildableWrapperPrefab);
            PvPBuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
            buildableWrapper.gameObject.SetActive(true);
            buildableWrapper.GetComponent<NetworkObject>().Spawn();
            buildableWrapper.StaticInitialise();
            buildableWrapper.Buildable.Initialise();
            return buildableWrapper.Buildable;
        }

        public static PvPCruiser GetCruiserPrefab(IPrefabKey hullKey)
        {
            return PvPPrefabCache.GetCruiser(hullKey);
        }

        public static PvPCruiser CreateCruiser(PvPCruiser cruiserPrefab, ulong ClientNetworkId, float x)
        {
            PvPCruiser cruiser = Object.Instantiate(cruiserPrefab, new Vector3(x, 0f, 0f), Quaternion.identity);
            cruiser.GetComponent<NetworkObject>().SpawnWithOwnership(ClientNetworkId, true);
            cruiser.GetComponent<NetworkObject>().DontDestroyWithOwner = false;
            cruiser.StaticInitialise();
            return cruiser;
        }
        public static PvPCruiser CreateAIBotCruiser(PvPCruiser cruiserPrefab, float x)
        {
            PvPCruiser cruiser = Object.Instantiate(cruiserPrefab, new Vector3(x, 0f, 0f), Quaternion.identity);
            cruiser.GetComponent<NetworkObject>().Spawn();
            cruiser.GetComponent<NetworkObject>().DontDestroyWithOwner = false;
            cruiser.StaticInitialise();
            cruiser.SetAIBotMode();
            return cruiser;
        }
        public static PvPCruiser CreateCruiser(string prefabName, ulong ClientNetworkId, float x)
        {
            PvPCruiser cruiser = GameObject.Instantiate(Resources.Load<PvPCruiser>("Hulls/" + prefabName), new Vector3(x, 0f, 0f), Quaternion.identity);
            cruiser.GetComponent<NetworkObject>().SpawnWithOwnership(ClientNetworkId, true);
            cruiser.GetComponent<NetworkObject>().DontDestroyWithOwner = false;
            cruiser.StaticInitialise();
            return cruiser;
        }

        public static IPoolable<Vector3> CreateExplosion(PvPExplosionType explosionType)
        {
            PvPExplosionController explosionPrefab = PvPPrefabCache.GetExplosion(PvPStaticPrefabKeys.PvPExplosions.GetKey(explosionType));
            PvPExplosionController newExplosion = Object.Instantiate(explosionPrefab);
            newExplosion.GetComponent<NetworkObject>().Spawn();

            IPoolable<Vector3> explosion = newExplosion.Initialise();
            explosion.Deactivated += (object sender, EventArgs e) => { explosionPool[(int)explosionType].Push(explosion); };

            return explosion;
        }

        public static IPoolable<Vector3> ShowExplosion(PvPExplosionType explosionType, Vector3 position)
        {
            IPoolable<Vector3> explosion;
            if (explosionPool != null && explosionPool[(int)explosionType].Count > 0)
                explosion = explosionPool[(int)explosionType].Pop();
            else
                explosion = CreateExplosion(explosionType);

            explosion.Activate(position);
            return explosion;
        }

        public static IPoolable<Vector3> CreateShipDeath(PvPShipDeathType deathType)
        {
            PvPShipDeathInitialiser shipDeathPrefab = PvPPrefabCache.GetShipDeath(PvPStaticPrefabKeys.PvPShipDeaths.GetKey(deathType));
            PvPShipDeathInitialiser newShipDeath = Object.Instantiate(shipDeathPrefab);
            newShipDeath.GetComponent<NetworkObject>().Spawn();

            IPoolable<Vector3> shipDeath = newShipDeath.CreateShipDeath();
            shipDeath.Deactivated += (object sender, EventArgs e) => { shipDeathPool[(int)deathType].Push(shipDeath); };

            return shipDeath;
        }

        public static IPoolable<Vector3> ShowShipDeath(PvPShipDeathType deathType, Vector3 position, Faction faction)
        {
            IPoolable<Vector3> shipDeath;

            if (shipDeathPool != null && shipDeathPool[(int)deathType].Count > 0)
                shipDeath = shipDeathPool[(int)deathType].Pop();
            else
                shipDeath = CreateShipDeath(deathType);

            shipDeath.Activate(position, faction);
            return shipDeath;
        }

        public static TProjectile CreateProjectile<TProjectile>(PvPProjectileType projectileType)
            where TProjectile : PvPProjectileControllerBase
        {
            PvPPrefab prefab = PvPPrefabCache.GetProjectile(PvPStaticPrefabKeys.PvPProjectiles.GetKey(projectileType));
            TProjectile projectile = (TProjectile)Object.Instantiate(prefab);
            projectile.GetComponent<NetworkObject>().Spawn();
            projectile.Initialise();
            projectile.Deactivated += (object sender, EventArgs e) => { projectilePool[(int)projectileType].Push(projectile); };

            return projectile;
        }

        public static TProjectile GetProjectile<TProjectile>(PvPProjectileType projectileType, ProjectileActivationArgs activationArgs)
            where TProjectile : PvPProjectileControllerBase
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
            PvPDroneController newDrone = Object.Instantiate(PvPPrefabCache.Drone);
            newDrone.GetComponent<NetworkObject>().Spawn();
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

        public static async Task<Bodykit> GetBodykit(IPrefabKey prefabKey)
        {
            string addressableKey = "Assets/Resources_moved/" + prefabKey.PrefabPath + ".prefab";
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(addressableKey);
            await handle.Task;
            if (handle.Status != AsyncOperationStatus.Succeeded
                || handle.Result == null)
            {
                throw new ArgumentException("Failed to retrieve prefab: " + addressableKey);
            }
            return handle.Result.GetComponent<Bodykit>();
        }

        public static async Task<VariantPrefab> GetVariant(IPrefabKey prefabKey)
        {
            string addressableKey = "Assets/Resources_moved/" + prefabKey.PrefabPath + ".prefab";
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(addressableKey);
            await handle.Task;
            if (handle.Status != AsyncOperationStatus.Succeeded || handle.Result == null)
            {
                throw new ArgumentException("Failed to retrieve prefab: " + addressableKey);
            }
            return handle.Result.GetComponent<VariantPrefab>();
        }
    }
}
