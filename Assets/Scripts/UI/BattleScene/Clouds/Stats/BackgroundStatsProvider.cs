using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundStatsProvider : IBackgroundStatsProvider
    {
        private readonly PrefabFetcher _prefabFetcher;

        public BackgroundStatsProvider(PrefabFetcher prefabFetcher)
        {
            Assert.IsNotNull(prefabFetcher);
            _prefabFetcher = prefabFetcher;
        }

        public async Task<PrefabContainer<BackgroundImageStats>> GetStatsAsyncLevel(int levelNum)
        {
            IPrefabKey key = new LevelBackgroundImageStatsKey(levelNum);
            return await _prefabFetcher.GetPrefabAsync<BackgroundImageStats>(key);
        }

        public async Task<PrefabContainer<BackgroundImageStats>> GetStatsAsyncSideQuest(int sideQuestID)
        {
            IPrefabKey key = new SideQuestBackgroundImageStatsKey(sideQuestID);
            return await _prefabFetcher.GetPrefabAsync<BackgroundImageStats>(key);
        }
    }
}