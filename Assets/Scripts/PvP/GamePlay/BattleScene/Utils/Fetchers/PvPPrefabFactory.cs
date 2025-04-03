using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Data.Models.PrefabKeys;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Object = UnityEngine.Object;
using BattleCruisers.Utils.Threading;
using BattleCruisers.Effects.Drones;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers
{
    public static class PvPPrefabFactory
    {
        public static IPvPBuildableWrapper<IPvPBuilding> GetBuildingWrapperPrefab(IPrefabKey buildingKey)
        {
            return PvPPrefabCache.GetBuilding(buildingKey);
        }

        public static IPvPBuilding CreateBuilding(
            IPvPBuildableWrapper<IPvPBuilding> buildingWrapperPrefab,
            PvPFactoryProvider factoryProvider,
            ulong clientID)
        {
            return CreateBuildingBuildable(buildingWrapperPrefab.UnityObject, factoryProvider, clientID);
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
            IPvPBuildableWrapper<IPvPUnit> unitWrapperPrefab,
            /* IPvPUIManager uiManager , */
            PvPFactoryProvider factoryProvider)
        {
            var _unitBuildable = CreateUnitBuildable(unitWrapperPrefab.UnityObject, factoryProvider);
            return _unitBuildable;
        }

        private static TBuildable CreateBuildingBuildable<TBuildable>(
            PvPBuildableWrapper<TBuildable> buildableWrapperPrefab,
            PvPFactoryProvider factoryProvider,
            ulong clientID) where TBuildable : class, IPvPBuilding
        {
            PvPHelper.AssertIsNotNull(buildableWrapperPrefab, factoryProvider);
            PvPBuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
            buildableWrapper.gameObject.SetActive(true);
            buildableWrapper.GetComponent<NetworkObject>().SpawnWithOwnership(clientID);
            buildableWrapper.GetComponent<NetworkObject>().DontDestroyWithOwner = false;
            buildableWrapper.StaticInitialise();
            buildableWrapper.Buildable.Initialise(factoryProvider);
            return buildableWrapper.Buildable;
        }
        public static PvPBuildableOutlineController CreateOutline(PvPBuildableOutlineController outlinePrefab)
        {
            PvPBuildableOutlineController outline = Object.Instantiate(outlinePrefab);
            outline.StaticInitialise();
            return outline;
        }

        private static TBuildable CreateUnitBuildable<TBuildable>(
            PvPBuildableWrapper<TBuildable> buildableWrapperPrefab,
            PvPFactoryProvider factoryProvider) where TBuildable : class, IPvPUnit
        {
            PvPHelper.AssertIsNotNull(buildableWrapperPrefab, factoryProvider);
            PvPBuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
            buildableWrapper.gameObject.SetActive(true);
            buildableWrapper.GetComponent<NetworkObject>().Spawn();
            buildableWrapper.StaticInitialise();
            buildableWrapper.Buildable.Initialise(factoryProvider);
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

        public static IPoolable<Vector3> CreateExplosion(PvPExplosionKey explosionKey)
        {
            PvPExplosionController explosionPrefab = PvPPrefabCache.GetExplosion(explosionKey);
            PvPExplosionController newExplosion = Object.Instantiate(explosionPrefab);
            newExplosion.GetComponent<NetworkObject>().Spawn();
            return newExplosion.Initialise();
        }

        public static IPoolable<Vector3> CreateShipDeath(PvPShipDeathKey shipDeathKey)
        {
            PvPShipDeathInitialiser shipDeathPrefab = PvPPrefabCache.GetShipDeath(shipDeathKey);
            PvPShipDeathInitialiser newShipDeath = Object.Instantiate(shipDeathPrefab);
            newShipDeath.GetComponent<NetworkObject>().Spawn();
            return newShipDeath.CreateShipDeath();
        }

        public static TProjectile CreateProjectile<TProjectile, TActiavtionArgs, TStats>(PvPProjectileKey prefabKey, PvPFactoryProvider factoryProvider)
            where TProjectile : PvPProjectileControllerBase<TActiavtionArgs, TStats>
            where TActiavtionArgs : ProjectileActivationArgs<TStats>
            where TStats : IProjectileStats
        {
            Assert.IsNotNull(factoryProvider);
            PvPPrefab prefab = PvPPrefabCache.GetProjectile(prefabKey);
            TProjectile projectile = (TProjectile)Object.Instantiate(prefab);
            projectile.GetComponent<NetworkObject>().Spawn();
            projectile.Initialise(factoryProvider);
            return projectile;
        }

        public static IDroneController CreateDrone()
        {
            PvPDroneController newDrone = Object.Instantiate(PvPPrefabCache.Drone);
            newDrone.GetComponent<NetworkObject>().Spawn();
            newDrone.StaticInitialise();
            return newDrone;
        }

        public static IPoolable<AudioSourceActivationArgs> CreateAudioSource(IDeferrer realTimeDeferrer)
        {
            Assert.IsNotNull(realTimeDeferrer);
            PvPAudioSourceInitialiser audioSourceInitialiser = Object.Instantiate(PvPPrefabCache.AudioSource);
            return audioSourceInitialiser.Initialise(realTimeDeferrer);
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
