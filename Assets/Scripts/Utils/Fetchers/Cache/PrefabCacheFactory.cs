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
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Fetchers.Cache
{
    /// <summary>
    /// Retrieving an addressable (such as a prefab) is an async operation.  To avoid async
    /// spread, this all prefabs are loaded at once (at the level start).  This means
    /// consuming code can remain synchronous.  Loading all prefabs takes about 6 seconds.
    /// </summary>
    /// PERF:  Only load prefabs required for level (ie, only 2 hulls, only unlocked buildables)
    public class PrefabCacheFactory : IPrefabCacheFactory
    {
        public async Task<IPrefabCache> CreatePrefabCacheAsync(IPrefabFetcher prefabFetcher)
        {
            Assert.IsNotNull(prefabFetcher);

            // Multiple prefab caches
            IMultiCache<BuildableWrapper<IBuilding>> buildings = await CreateMultiCacheAsync<BuildableWrapper<IBuilding>>(prefabFetcher, StaticPrefabKeys.Buildings.AllKeys);
            IMultiCache<BuildableWrapper<IUnit>> units = await CreateMultiCacheAsync<BuildableWrapper<IUnit>>(prefabFetcher, StaticPrefabKeys.Units.AllKeys);
            IMultiCache<Cruiser> cruisers = await CreateMultiCacheAsync<Cruiser>(prefabFetcher, StaticPrefabKeys.Hulls.AllKeys);
            IMultiCache<ExplosionController> explosions = await CreateMultiCacheAsync<ExplosionController>(prefabFetcher, StaticPrefabKeys.Explosions.AllKeys);

            // Multiple untyped prefab caches
            IUntypedMultiCache<Projectile> projectiles = await CreateUntypedMultiCacheAsync<Projectile>(prefabFetcher, StaticPrefabKeys.Projectiles.AllKeys);

            // Single prefab caches
            CountdownController countdown = await prefabFetcher.GetPrefabAsync<CountdownController>(StaticPrefabKeys.UI.DeleteCountdown);
            DroneController drone = await prefabFetcher.GetPrefabAsync<DroneController>(StaticPrefabKeys.Effects.BuilderDrone);

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
            IDictionary<IPrefabKey, TPrefab> _keyToPrefab = await CreatePrefabs<TPrefab>(prefabFetcher, prefabKeys);
            return new MultiCache<TPrefab>(_keyToPrefab);
        }

        private async Task<IUntypedMultiCache<TPrefabBase>> CreateUntypedMultiCacheAsync<TPrefabBase>(IPrefabFetcher prefabFetcher, IList<IPrefabKey> prefabKeys)
            where TPrefabBase : class
        {
            IDictionary<IPrefabKey, TPrefabBase> _keyToPrefab = await CreatePrefabs<TPrefabBase>(prefabFetcher, prefabKeys);
            return new UntypedMultiCache<TPrefabBase>(_keyToPrefab);
        }

        private async Task<IDictionary<IPrefabKey, TPrefab>> CreatePrefabs<TPrefab>(
            IPrefabFetcher prefabFetcher, 
            IList<IPrefabKey> prefabKeys) 
                where TPrefab : class
        {
            IDictionary<IPrefabKey, TPrefab> _keyToPrefab = new Dictionary<IPrefabKey, TPrefab>();

            foreach (IPrefabKey prefabKey in prefabKeys)
            {
                TPrefab prefab = await prefabFetcher.GetPrefabAsync<TPrefab>(prefabKey);
                _keyToPrefab.Add(prefabKey, prefab);
            }

            return _keyToPrefab;
        }
    }
}