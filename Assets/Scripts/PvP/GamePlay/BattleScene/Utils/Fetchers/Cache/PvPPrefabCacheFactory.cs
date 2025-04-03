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
using BattleCruisers.Utils.Fetchers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache
{
    /// <summary>
    /// Retrieving an addressable (such as a prefab) is an async operation.  To avoid async
    /// spread, this all prefabs are loaded at once (at the level start).  This means
    /// consuming code can remain synchronous.  Loading all prefabs takes about 1 seconds.
    /// </summary>
    /// PERF:  Only load prefabs required for level (ie, only 2 hulls, only unlocked buildables)
    public class PvPPrefabCacheFactory
    {
        public async Task<PvPPrefabCache> CreatePrefabCacheAsync()
        {
            IList<Task> retrievePrefabsTasks = new List<Task>();

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
            // Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After retrieve all prefabs task");

            return
                new PvPPrefabCache(
                    new PvPMultiCache<PvPBuildableWrapper<IPvPBuilding>>(keyToBuilding),
                    new PvPMultiCache<PvPBuildableWrapper<IPvPUnit>>(keyToUnit),
                    new PvPMultiCache<PvPCruiser>(keyToCruiser),
                    new PvPMultiCache<PvPExplosionController>(keyToExplosion),
                    new PvPMultiCache<PvPShipDeathInitialiser>(keyToDeath),
                    new PvPMultiCache<PvPPrefab>(keyToProjectile),
                    droneContainer.Value,
                    audioSourceContainer.Value,
                    new PvPMultiCache<PvPBuildableOutlineController>(keyToOutline));
        }

        private async Task GetPrefabs<TPrefab>(
            IList<IPrefabKey> prefabKeys,
            IDictionary<IPrefabKey, TPrefab> keyToPrefab)
                where TPrefab : class, IPrefab
        {
            IEnumerable<Task> prefabTasks = prefabKeys.Select(prefabKey => GetPrefab(keyToPrefab, prefabKey));
            await Task.WhenAll(prefabTasks);
        }

        private async Task GetPrefab<TPrefab>(
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

        private async Task GetPrefab<TPrefab>(
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