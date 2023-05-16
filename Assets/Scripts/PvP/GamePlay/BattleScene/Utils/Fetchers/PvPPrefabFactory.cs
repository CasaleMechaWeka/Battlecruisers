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
            IPvPFactoryProvider factoryProvider)
        {
            return CreateBuildable(buildingWrapperPrefab.UnityObject, uiManager, factoryProvider);
        }

        public IPvPBuildableWrapper<IPvPUnit> GetUnitWrapperPrefab(IPvPPrefabKey unitKey)
        {
            return _prefabCache.GetUnit(unitKey);
        }

        public IPvPUnit CreateUnit(
            IPvPBuildableWrapper<IPvPUnit> unitWrapperPrefab,
            IPvPUIManager uiManager,
            IPvPFactoryProvider factoryProvider)
        {
            return CreateBuildable(unitWrapperPrefab.UnityObject, uiManager, factoryProvider);
        }

        private TBuildable CreateBuildable<TBuildable>(
            PvPBuildableWrapper<TBuildable> buildableWrapperPrefab,
            IPvPUIManager uiManager,
            IPvPFactoryProvider factoryProvider) where TBuildable : class, IPvPBuildable
        {
            PvPHelper.AssertIsNotNull(buildableWrapperPrefab, uiManager, factoryProvider);

            PvPBuildableWrapper<TBuildable> buildableWrapper = Object.Instantiate(buildableWrapperPrefab);
            buildableWrapper.gameObject.SetActive(true);
            buildableWrapper.StaticInitialise(_commonStrings);
            buildableWrapper.Buildable.Initialise(uiManager, factoryProvider);

            // Logging.Log(Tags.PREFAB_FACTORY, $"Building: {buildableWrapper.Buildable}  Prefab id: {buildableWrapperPrefab.GetInstanceID()}  New instance id: {buildableWrapper.GetInstanceID()}");
            return buildableWrapper.Buildable;
        }

        public PvPCruiser GetCruiserPrefab(IPvPPrefabKey hullKey)
        {
            return _prefabCache.GetCruiser(hullKey);
        }

        public PvPCruiser CreateCruiser(PvPCruiser cruiserPrefab)
        {
            PvPCruiser cruiser = Object.Instantiate(cruiserPrefab);
            cruiser.StaticInitialise(_commonStrings);
            return cruiser;
        }

        public IPvPExplosion CreateExplosion(PvPExplosionKey explosionKey)
        {
            PvPExplosionController explosionPrefab = _prefabCache.GetExplosion(explosionKey);
            PvPExplosionController newExplosion = Object.Instantiate(explosionPrefab);
            return newExplosion.Initialise();
        }

        public IPvPShipDeath CreateShipDeath(PvPShipDeathKey shipDeathKey)
        {
            PvPShipDeathInitialiser shipDeathPrefab = _prefabCache.GetShipDeath(shipDeathKey);
            PvPShipDeathInitialiser newShipDeath = Object.Instantiate(shipDeathPrefab);
            return newShipDeath.CreateShipDeath();
        }

        public TProjectile CreateProjectile<TProjectile, TActiavtionArgs, TStats>(PvPProjectileKey prefabKey, IPvPFactoryProvider factoryProvider)
            where TProjectile : PvPProjectileControllerBase<TActiavtionArgs, TStats>
            where TActiavtionArgs : PvPProjectileActivationArgs<TStats>
            where TStats : IPvPProjectileStats
        {
            Assert.IsNotNull(factoryProvider);

            TProjectile prefab = _prefabCache.GetProjectile<TProjectile>(prefabKey);
            TProjectile projectile = Object.Instantiate(prefab);
            projectile.Initialise(_commonStrings, factoryProvider);
            return projectile;
        }

        public IPvPDroneController CreateDrone()
        {
            PvPDroneController newDrone = Object.Instantiate(_prefabCache.Drone);
            newDrone.StaticInitialise(_commonStrings);
            return newDrone;
        }

        public IPvPAudioSourcePoolable CreateAudioSource(IPvPDeferrer realTimeDeferrer)
        {
            Assert.IsNotNull(realTimeDeferrer);

            PvPAudioSourceInitialiser audioSourceInitialiser = Object.Instantiate(_prefabCache.AudioSource);
            return audioSourceInitialiser.Initialise(realTimeDeferrer, _settingsManager);
        }
    }
}
