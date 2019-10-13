using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.Utils.Threading;
using System.Threading.Tasks;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class HelperFactory
    {
        private const int DEFAULT_NUM_OF_DRONES = 10;

        public static async Task<Helper> CreateHelperAsync(
            int numOfDrones = DEFAULT_NUM_OF_DRONES,
            float buildSpeedMultiplier = BuildSpeedMultipliers.VERY_FAST,
            IDeferrer deferrer = null,
            IUpdaterProvider updaterProvider = null)
        {
            PrefabCacheFactory prefabCacheFactory = new PrefabCacheFactory();
            IPrefabCache prefabCache = await prefabCacheFactory.CreatePrefabCacheAsync(new PrefabFetcher());
            IPrefabFactory prefabFactory = new PrefabFactoryNEW(prefabCache);

            return
                new Helper(
                    numOfDrones,
                    buildSpeedMultiplier,
                    deferrer,
                    updaterProvider,
                    prefabFactory);
        }

        public static Helper CreateHelperNoPrefabFactory(
            int numOfDrones = DEFAULT_NUM_OF_DRONES,
            float buildSpeedMultiplier = BuildSpeedMultipliers.VERY_FAST,
            IDeferrer deferrer = null,
            IUpdaterProvider updaterProvider = null)
        {
            return
                new Helper(
                    numOfDrones,
                    buildSpeedMultiplier,
                    deferrer,
                    updaterProvider,
                    prefabFactory: null);
        }
    }
}