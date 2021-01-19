using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundStatsProvider : IBackgroundStatsProvider
    {
        private readonly IPrefabFetcher _prefabFetcher;

        public BackgroundStatsProvider(IPrefabFetcher prefabFetcher)
        {
            Assert.IsNotNull(prefabFetcher);
            _prefabFetcher = prefabFetcher;
        }

        public async Task<IPrefabContainer<BackgroundImageStats>> GetStatsAsync(int levelNum)
        {
            IPrefabKey key = new BackgroundImageStatsKey(levelNum);
            return await _prefabFetcher.GetPrefabAsync<BackgroundImageStats>(key);
        }
    }
}