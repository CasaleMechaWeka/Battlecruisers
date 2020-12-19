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
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Timers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Fetchers.Cache
{
    /// <summary>
    /// Retrieving an addressable (such as a prefab) is an async operation.  To avoid async
    /// spread, this all prefabs are loaded at once (at the level start).  This means
    /// consuming code can remain synchronous.  Loading all prefabs takes about 1 seconds.
    /// </summary>
    /// PERF:  Only load prefabs required for level (ie, only 2 hulls, only unlocked buildables)
    public class PrefabCacheFactory : IPrefabCacheFactory
    {
        public async Task<IPrefabCache> CreatePrefabCacheAsync(IPrefabFetcher prefabFetcher)
        {
            Assert.IsNotNull(prefabFetcher);

            IList<Task> retrievePrefabsTasks = new List<Task>();

            IDictionary<IPrefabKey, BuildableWrapper<IBuilding>> keyToBuilding = new ConcurrentDictionary<IPrefabKey, BuildableWrapper<IBuilding>>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, StaticPrefabKeys.Buildings.AllKeys, keyToBuilding));

            IDictionary<IPrefabKey, BuildableWrapper<IUnit>> keyToUnit = new ConcurrentDictionary<IPrefabKey, BuildableWrapper<IUnit>>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, StaticPrefabKeys.Units.AllKeys, keyToUnit));

            IDictionary<IPrefabKey, Cruiser> keyToCruiser = new ConcurrentDictionary<IPrefabKey, Cruiser>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, StaticPrefabKeys.Hulls.AllKeys, keyToCruiser));

            IDictionary<IPrefabKey, ExplosionController> keyToExplosion = new ConcurrentDictionary<IPrefabKey, ExplosionController>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, StaticPrefabKeys.Explosions.AllKeys, keyToExplosion));
            
            IDictionary<IPrefabKey, ShipDeathInitialiser> keyToDeath = new ConcurrentDictionary<IPrefabKey, ShipDeathInitialiser>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, StaticPrefabKeys.ShipDeaths.AllKeys, keyToDeath));

            IDictionary<IPrefabKey, Projectile> keyToProjectile = new ConcurrentDictionary<IPrefabKey, Projectile>();
            retrievePrefabsTasks.Add(GetPrefabs(prefabFetcher, StaticPrefabKeys.Projectiles.AllKeys, keyToProjectile));

            Container<CountdownController> countdownContainer = new Container<CountdownController>();
            retrievePrefabsTasks.Add(GetPrefab(prefabFetcher, StaticPrefabKeys.UI.DeleteCountdown, countdownContainer));

            Container<DroneController> droneContainer = new Container<DroneController>();
            retrievePrefabsTasks.Add(GetPrefab(prefabFetcher, StaticPrefabKeys.Effects.BuilderDrone, droneContainer));

            Container<AudioSourceInitialiser> audioSourceContainer = new Container<AudioSourceInitialiser>();
            retrievePrefabsTasks.Add(GetPrefab(prefabFetcher, StaticPrefabKeys.AudioSource, audioSourceContainer));

            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre rertieve all prefabs task");
            await Task.WhenAll(retrievePrefabsTasks);
            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After rertieve all prefabs task");

            return
                new PrefabCache(
                    new MultiCache<BuildableWrapper<IBuilding>>(keyToBuilding),
                    new MultiCache<BuildableWrapper<IUnit>>(keyToUnit),
                    new MultiCache<Cruiser>(keyToCruiser),
                    new MultiCache<ExplosionController>(keyToExplosion),
                    new MultiCache<ShipDeathInitialiser>(keyToDeath),
                    new UntypedMultiCache<Projectile>(keyToProjectile),
                    countdownContainer.Value,
                    droneContainer.Value,
                    audioSourceContainer.Value);
        }

        private async Task GetPrefabs<TPrefab>(
            IPrefabFetcher prefabFetcher,
            IList<IPrefabKey> prefabKeys,
            IDictionary<IPrefabKey, TPrefab> keyToPrefab)
                where TPrefab : class, IPrefab
        {
            IEnumerable<Task> prefabTasks = prefabKeys.Select(prefabKey => GetPrefab(prefabFetcher, keyToPrefab, prefabKey));
            await Task.WhenAll(prefabTasks);
        }

        private async Task GetPrefab<TPrefab>( 
            IPrefabFetcher prefabFetcher,
            IDictionary<IPrefabKey, TPrefab> keyToPrefab, 
            IPrefabKey prefabKey)
                where TPrefab : class, IPrefab
        {
            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre GetPrefabAsync");
            TPrefab prefab = await prefabFetcher.GetPrefabAsync<TPrefab>(prefabKey);
            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After GetPrefabAsync");

            prefab.StaticInitialise();
            keyToPrefab.Add(prefabKey, prefab);
        }

        private async Task GetPrefab<TPrefab>(
            IPrefabFetcher prefabFetcher,
            IPrefabKey prefabKey,
            Container<TPrefab> prefabContainer)
                where TPrefab : class, IPrefab
        {
            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "Pre GetPrefabAsync");
            prefabContainer.Value = await prefabFetcher.GetPrefabAsync<TPrefab>(prefabKey);
            Logging.Log(Tags.PREFAB_CACHE_FACTORY, "After GetPrefabAsync");

            prefabContainer.Value.StaticInitialise();
        }
    }
}