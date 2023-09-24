using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using System.Threading.Tasks;
using BattleCruisers.Buildables;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Projectiles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using static BattleCruisers.Data.Static.StaticPrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers
{
    public class PvPPrefabFactory : IPvPPrefabFactory
    {
        private readonly IPvPPrefabCache _prefabCache;
        private readonly ISettingsManager _settingsManager;
        private readonly ILocTable _commonStrings;

        public PvPPrefabFactory(IPvPPrefabCache prefabCache, ISettingsManager settingsManager, ILocTable commonStrings)
        {
            PvPHelper.AssertIsNotNull(prefabCache, settingsManager, commonStrings);

            _prefabCache = prefabCache;
            _settingsManager = settingsManager;
            _commonStrings = commonStrings;
        }

        public IPvPBuildableWrapper<IPvPBuilding> GetBuildingWrapperPrefab(IPvPPrefabKey buildingKey)
        {
            return _prefabCache.GetBuilding(buildingKey);
        }

        public IPvPBuilding CreateBuilding(
            IPvPBuildableWrapper<IPvPBuilding> buildingWrapperPrefab,
            IPvPUIManager uiManager,
            IPvPFactoryProvider factoryProvider,
            ulong clientID)
        {
            return CreateBuildingBuildable(buildingWrapperPrefab.UnityObject, factoryProvider, clientID);
        }

        public IPvPBuildableWrapper<IPvPUnit> GetUnitWrapperPrefab(IPvPPrefabKey unitKey)
        {
            return _prefabCache.GetUnit(unitKey);
        }

        public PvPBuildableOutlineController GetOutline(IPvPPrefabKey outlineKey)
        {
            return _prefabCache.GetOutline(outlineKey);
        }

        public async Task<IPvPUnit> CreateUnit(
            IPvPBuildableWrapper<IPvPUnit> unitWrapperPrefab,
            /* IPvPUIManager uiManager , */
            IPvPFactoryProvider factoryProvider)
        {
            var _unitBuildable = await CreateUnitBuildable(unitWrapperPrefab.UnityObject, factoryProvider);
            return _unitBuildable;
        }

        private TBuildable CreateBuildingBuildable<TBuildable>(
            PvPBuildableWrapper<TBuildable> buildableWrapperPrefab,
            IPvPFactoryProvider factoryProvider,
            ulong clientID) where TBuildable : class, IPvPBuilding
        {
            PvPHelper.AssertIsNotNull(buildableWrapperPrefab, factoryProvider);
            PvPBuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
            buildableWrapper.GetComponent<NetworkObject>().SpawnWithOwnership(clientID);
            buildableWrapper.GetComponent<NetworkObject>().DontDestroyWithOwner = false;
            buildableWrapper.gameObject.SetActive(true);
            buildableWrapper.StaticInitialise(_commonStrings);
            buildableWrapper.Buildable.Initialise(factoryProvider);
            return buildableWrapper.Buildable;
        }
        public PvPBuildableOutlineController CreateOutline(PvPBuildableOutlineController outlinePrefab)
        {
            PvPBuildableOutlineController outline = Object.Instantiate(outlinePrefab);
            outline.StaticInitialise(_commonStrings);
            return outline;
        }

        private async Task<TBuildable> CreateUnitBuildable<TBuildable>(
            PvPBuildableWrapper<TBuildable> buildableWrapperPrefab,
            IPvPFactoryProvider factoryProvider) where TBuildable : class, IPvPUnit
        {
            PvPHelper.AssertIsNotNull(buildableWrapperPrefab, factoryProvider);
            PvPBuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
            if (buildableWrapper != null && buildableWrapper.GetComponent<NetworkObject>() != null)
            {
                buildableWrapper.GetComponent<NetworkObject>().Spawn();
            }
            else
            {
                await Task.Delay(10);
                buildableWrapper.GetComponent<NetworkObject>().Spawn();
            }

            buildableWrapper.gameObject.SetActive(true);
            buildableWrapper.StaticInitialise(_commonStrings);
            buildableWrapper.Buildable.Initialise(factoryProvider);
            return buildableWrapper.Buildable;
        }

        public PvPCruiser GetCruiserPrefab(IPvPPrefabKey hullKey)
        {
            return _prefabCache.GetCruiser(hullKey);
        }

        public PvPCruiser CreateCruiser(PvPCruiser cruiserPrefab, ulong ClientNetworkId, float x)
        {
            PvPCruiser cruiser = Object.Instantiate(cruiserPrefab, new Vector3(x, 0f, 0f), Quaternion.identity);
            cruiser.GetComponent<NetworkObject>().SpawnWithOwnership(ClientNetworkId, true);
            cruiser.GetComponent<NetworkObject>().DontDestroyWithOwner = false;
            cruiser.StaticInitialise(_commonStrings);
            return cruiser;
        }

        public PvPCruiser CreateCruiser(string prefabName, ulong ClientNetworkId, float x)
        {
            PvPCruiser cruiser = GameObject.Instantiate(Resources.Load<PvPCruiser>("Hulls/" + prefabName), new Vector3(x, 0f, 0f), Quaternion.identity);
            cruiser.GetComponent<NetworkObject>().SpawnWithOwnership(ClientNetworkId, true);
            cruiser.GetComponent<NetworkObject>().DontDestroyWithOwner = false;
            cruiser.StaticInitialise(_commonStrings);
            return cruiser;
        }

        public async Task<IPvPExplosion> CreateExplosion(PvPExplosionKey explosionKey)
        {
            PvPExplosionController explosionPrefab = _prefabCache.GetExplosion(explosionKey);
            PvPExplosionController newExplosion = Object.Instantiate(explosionPrefab);
            if (newExplosion != null && newExplosion.GetComponent<NetworkObject>() != null)
                newExplosion.GetComponent<NetworkObject>().Spawn();
            else
            {
                await Task.Delay(10);
                newExplosion.GetComponent<NetworkObject>().Spawn();
            }
            return newExplosion.Initialise();
        }

        public async Task<IPvPShipDeath> CreateShipDeath(PvPShipDeathKey shipDeathKey)
        {
            PvPShipDeathInitialiser shipDeathPrefab = _prefabCache.GetShipDeath(shipDeathKey);
            PvPShipDeathInitialiser newShipDeath = Object.Instantiate(shipDeathPrefab);
            if (newShipDeath != null && newShipDeath.GetComponent<NetworkObject>() != null)
                newShipDeath.GetComponent<NetworkObject>().Spawn();
            else
            {
                await Task.Delay(10);
                newShipDeath.GetComponent<NetworkObject>().Spawn();
            }
            return newShipDeath.CreateShipDeath();
        }

        public async Task<TProjectile> CreateProjectile<TProjectile, TActiavtionArgs, TStats>(PvPProjectileKey prefabKey, IPvPFactoryProvider factoryProvider)
            where TProjectile : PvPProjectileControllerBase<TActiavtionArgs, TStats>
            where TActiavtionArgs : PvPProjectileActivationArgs<TStats>
            where TStats : IPvPProjectileStats
        {
            Assert.IsNotNull(factoryProvider);
            TProjectile prefab = _prefabCache.GetProjectile<TProjectile>(prefabKey);
            TProjectile projectile = Object.Instantiate(prefab);
            if (projectile != null && projectile.GetComponent<NetworkObject>() != null)
            {
                projectile.GetComponent<NetworkObject>().Spawn();
            }
            else
            {
                await Task.Delay(10);
                projectile.GetComponent<NetworkObject>().Spawn();
            }
            projectile.Initialise(_commonStrings, factoryProvider);
            return projectile;
        }

        public async Task<IPvPDroneController> CreateDrone()
        {
            PvPDroneController newDrone = Object.Instantiate(_prefabCache.Drone);
            if (newDrone != null && newDrone.GetComponent<NetworkObject>() != null)
                newDrone.GetComponent<NetworkObject>().Spawn();
            else
            {
                await Task.Delay(10);
                newDrone.GetComponent<NetworkObject>().Spawn();
            }
            newDrone.StaticInitialise(_commonStrings);
            return newDrone;
        }

        public async Task<IPvPAudioSourcePoolable> CreateAudioSource(IPvPDeferrer realTimeDeferrer)
        {
            Assert.IsNotNull(realTimeDeferrer);
            PvPAudioSourceInitialiser audioSourceInitialiser = Object.Instantiate(_prefabCache.AudioSource);
            return audioSourceInitialiser.Initialise(realTimeDeferrer, _settingsManager);
        }

        public PvPPrefab GetPrefab(string prefabPath)
        {
            return _prefabCache.GetPrefab(prefabPath);
        }
    }
}
