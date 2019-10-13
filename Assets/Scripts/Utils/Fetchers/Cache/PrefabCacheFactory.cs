using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Projectiles;
using BattleCruisers.Utils.Timers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Fetchers.Cache
{
    /// <summary>
    /// Retrieving an addressable (such as a prefab) is an async operation.  To avoid async
    /// spread, this all prefabs are loaded at once (at the level start).  This means
    /// consuming code can remain synchronous.  Loading all prefabs takes about 2 seconds.
    /// </summary>
    /// PERF:  Only load prefabs required for level (ie, only 2 hulls, only unlocked buildables)
    public class PrefabCacheFactory : IPrefabCacheFactory
    {
        public async Task<IPrefabCache> CreatePrefabCacheAsync(IPrefabFetcher prefabFetcher)
        {
            Debug.Log("CreatePrefabCacheAsync()  START  0 of 7");
            Assert.IsNotNull(prefabFetcher);

            // Multiple prefab caches
            IMultiCache<BuildableWrapper<IBuilding>> buildings = await CreateMultiCacheAsync<BuildableWrapper<IBuilding>>(prefabFetcher, StaticPrefabKeys.Buildings.AllKeys);
            Debug.Log("CreatePrefabCacheAsync()  1 of 7");

            IMultiCache<BuildableWrapper<IUnit>> units = await CreateMultiCacheAsync<BuildableWrapper<IUnit>>(prefabFetcher, StaticPrefabKeys.Units.AllKeys);
            Debug.Log("CreatePrefabCacheAsync()  2 of 7");

            IMultiCache<Cruiser> cruisers = await CreateMultiCacheAsync<Cruiser>(prefabFetcher, StaticPrefabKeys.Hulls.AllKeys);
            Debug.Log("CreatePrefabCacheAsync()  3 of 7");

            IMultiCache<ExplosionController> explosions = await CreateMultiCacheAsync<ExplosionController>(prefabFetcher, StaticPrefabKeys.Explosions.AllKeys);
            Debug.Log("CreatePrefabCacheAsync()  4 of 7");

            // Multiple untyped prefab caches
            IUntypedMultiCache<Projectile> projectiles = await CreateUntypedMultiCacheAsync<Projectile>(prefabFetcher, StaticPrefabKeys.Projectiles.AllKeys);
            Debug.Log("CreatePrefabCacheAsync()  5 of 7");

            // Single prefab caches
            CountdownController countdown = await prefabFetcher.GetPrefabAsync<CountdownController>(StaticPrefabKeys.UI.DeleteCountdown);
            Debug.Log("CreatePrefabCacheAsync()  6 of 7");

            DroneController drone = await prefabFetcher.GetPrefabAsync<DroneController>(StaticPrefabKeys.Effects.BuilderDrone);
            Debug.Log("CreatePrefabCacheAsync()  END  7 of 7");

            return
                new PrefabCache(
                    buildings,
                    units,
                    cruisers,
                    explosions,
                    projectiles,
                    countdown,
                    drone);
        }

        private async Task<IMultiCache<TPrefab>> CreateMultiCacheAsync<TPrefab>(IPrefabFetcher prefabFetcher, IList<IPrefabKey> prefabKeys)
            where TPrefab : class
        {
            IDictionary<IPrefabKey, TPrefab> _keyToPrefab = await GetPrefabs<TPrefab>(prefabFetcher, prefabKeys);
            return new MultiCache<TPrefab>(_keyToPrefab);
        }

        private async Task<IUntypedMultiCache<TPrefabBase>> CreateUntypedMultiCacheAsync<TPrefabBase>(IPrefabFetcher prefabFetcher, IList<IPrefabKey> prefabKeys)
            where TPrefabBase : class
        {
            IDictionary<IPrefabKey, TPrefabBase> _keyToPrefab = await GetPrefabs<TPrefabBase>(prefabFetcher, prefabKeys);
            return new UntypedMultiCache<TPrefabBase>(_keyToPrefab);
        }

        private async Task<IDictionary<IPrefabKey, TPrefab>> GetPrefabs<TPrefab>(
            IPrefabFetcher prefabFetcher, 
            IList<IPrefabKey> prefabKeys) 
                where TPrefab : class
        {
            IDictionary<IPrefabKey, TPrefab> keyToPrefab = new ConcurrentDictionary<IPrefabKey, TPrefab>();

            IEnumerable<Task> prefabTasks = prefabKeys.Select(prefabKey => GetPrefab(prefabFetcher, keyToPrefab, prefabKey));
            await Task.WhenAll(prefabTasks);
            return keyToPrefab;
        }

        private async Task GetPrefab<TPrefab>( 
            IPrefabFetcher prefabFetcher,
            IDictionary<IPrefabKey, TPrefab> keyToPrefab, 
            IPrefabKey prefabKey)
                where TPrefab : class
        {
            TPrefab prefab = await prefabFetcher.GetPrefabAsync<TPrefab>(prefabKey);
            keyToPrefab.Add(prefabKey, prefab);
        }
    }
}