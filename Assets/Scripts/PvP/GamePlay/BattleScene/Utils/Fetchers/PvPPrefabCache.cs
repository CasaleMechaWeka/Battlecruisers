using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers
{
    public static class PvPPrefabCache
    {
        private static MultiCache<PvPBuildableWrapper<IPvPBuilding>> _buildings;
        private static MultiCache<PvPBuildableWrapper<IPvPUnit>> _units;
        private static MultiCache<PvPCruiser> _cruisers;
        private static MultiCache<PvPExplosionController> _explosions;
        private static MultiCache<PvPShipDeathInitialiser> _shipDeaths;
        private static MultiCache<PvPPrefab> _projectiles;
        private static MultiCache<PvPBuildableOutlineController> _outlines;

        public static PvPDroneController Drone { get; private set; }
        public static PvPAudioSourceInitialiser AudioSource { get; private set; }

        public static PvPBuildableWrapper<IPvPBuilding> GetBuilding(IPrefabKey key) => _buildings.GetPrefab(key);
        public static PvPBuildableOutlineController GetOutline(IPrefabKey key) => _outlines.GetPrefab(key);
        public static PvPBuildableWrapper<IPvPUnit> GetUnit(IPrefabKey key) => _units.GetPrefab(key);
        public static PvPCruiser GetCruiser(IPrefabKey key) => _cruisers.GetPrefab(key);
        public static PvPExplosionController GetExplosion(IPrefabKey key) => _explosions.GetPrefab(key);
        public static PvPShipDeathInitialiser GetShipDeath(IPrefabKey key) => _shipDeaths.GetPrefab(key);
        public static PvPPrefab GetProjectile(IPrefabKey prefabKey) => _projectiles.GetPrefab(prefabKey);

        public static async Task CreatePvPPrefabCacheAsync()
        {
            List<Task> retrievePrefabsTasks = new List<Task>();
            IDictionary<IPrefabKey, PvPBuildableWrapper<IPvPBuilding>> keyToBuilding = new ConcurrentDictionary<IPrefabKey, PvPBuildableWrapper<IPvPBuilding>>();
            retrievePrefabsTasks.Add(GetPrefabs(PvPStaticPrefabKeys.PvPBuildings.AllKeys, keyToBuilding));
            IDictionary<IPrefabKey, PvPBuildableWrapper<IPvPUnit>> keyToUnit = new ConcurrentDictionary<IPrefabKey, PvPBuildableWrapper<IPvPUnit>>();
            retrievePrefabsTasks.Add(GetPrefabs(PvPStaticPrefabKeys.PvPUnits.AllKeys, keyToUnit));
            IDictionary<IPrefabKey, PvPCruiser> keyToCruiser = new ConcurrentDictionary<IPrefabKey, PvPCruiser>();
            retrievePrefabsTasks.Add(GetPrefabs(PvPStaticPrefabKeys.PvPHulls.AllKeys, keyToCruiser));
            IDictionary<IPrefabKey, PvPExplosionController> keyToExplosion = new ConcurrentDictionary<IPrefabKey, PvPExplosionController>();
            retrievePrefabsTasks.Add(GetPrefabs(PvPStaticPrefabKeys.PvPExplosions.AllKeys, keyToExplosion));
            IDictionary<IPrefabKey, PvPShipDeathInitialiser> keyToDeath = new ConcurrentDictionary<IPrefabKey, PvPShipDeathInitialiser>();
            retrievePrefabsTasks.Add(GetPrefabs(PvPStaticPrefabKeys.PvPShipDeaths.AllKeys, keyToDeath));
            IDictionary<IPrefabKey, PvPPrefab> keyToProjectile = new ConcurrentDictionary<IPrefabKey, PvPPrefab>();
            retrievePrefabsTasks.Add(GetPrefabs(PvPStaticPrefabKeys.PvPProjectiles.AllKeys, keyToProjectile));
            Container<PvPDroneController> droneContainer = new Container<PvPDroneController>();
            retrievePrefabsTasks.Add(GetPrefab(PvPStaticPrefabKeys.PvPEffects.PvPBuilderDrone, droneContainer));
            Container<PvPAudioSourceInitialiser> audioSourceContainer = new Container<PvPAudioSourceInitialiser>();
            retrievePrefabsTasks.Add(GetPrefab(PvPStaticPrefabKeys.AudioSource, audioSourceContainer));
            IDictionary<IPrefabKey, PvPBuildableOutlineController> keyToOutline = new ConcurrentDictionary<IPrefabKey, PvPBuildableOutlineController>();
            retrievePrefabsTasks.Add(GetPrefabs(PvPStaticPrefabKeys.PvPBuildableOutlines.AllKeys, keyToOutline));
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre retrieve all prefabs task");
            await Task.WhenAll(retrievePrefabsTasks);

            _buildings = new MultiCache<PvPBuildableWrapper<IPvPBuilding>>(keyToBuilding);
            _units = new MultiCache<PvPBuildableWrapper<IPvPUnit>>(keyToUnit);
            _cruisers = new MultiCache<PvPCruiser>(keyToCruiser);
            _explosions = new MultiCache<PvPExplosionController>(keyToExplosion);
            _shipDeaths = new MultiCache<PvPShipDeathInitialiser>(keyToDeath);
            _projectiles = new MultiCache<PvPPrefab>(keyToProjectile);
            Drone = droneContainer.Value;
            AudioSource = audioSourceContainer.Value;
            _outlines = new MultiCache<PvPBuildableOutlineController>(keyToOutline);
        }

        public static void Clear()
        {
            Debug.Log("CLEARED PVP CACHE");
            _buildings = null;
            _units = null;
            _cruisers = null;
            _explosions = null;
            _shipDeaths = null;
            _projectiles = null;
            _outlines = null;
        }

        private static async Task GetPrefabs<TPrefab>(
    IList<IPrefabKey> prefabKeys,
    IDictionary<IPrefabKey, TPrefab> keyToPrefab)
        where TPrefab : class, IPrefab
        {
            IEnumerable<Task> prefabTasks = prefabKeys.Select(prefabKey => GetPrefab(keyToPrefab, prefabKey));
            await Task.WhenAll(prefabTasks);
        }

        private static async Task GetPrefab<TPrefab>(
            IDictionary<IPrefabKey, TPrefab> keyToPrefab,
            IPrefabKey prefabKey)
                where TPrefab : class, IPrefab
        {
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre GetPrefabAsync");
            PrefabContainer<TPrefab> prefabContainer = await PrefabFetcher.GetPrefabAsync<TPrefab>(prefabKey);
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After GetPrefabAsync");
            prefabContainer.Prefab.StaticInitialise();
            keyToPrefab.Add(prefabKey, prefabContainer.Prefab);
        }

        private static async Task GetPrefab<TPrefab>(
            IPrefabKey prefabKey,
            Container<TPrefab> prefabContainer)
                where TPrefab : class, IPrefab
        {
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre GetPrefabAsync");
            PrefabContainer<TPrefab> result = await PrefabFetcher.GetPrefabAsync<TPrefab>(prefabKey);
            prefabContainer.Value = result.Prefab;
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After GetPrefabAsync");

            prefabContainer.Value.StaticInitialise();
        }
    }
}