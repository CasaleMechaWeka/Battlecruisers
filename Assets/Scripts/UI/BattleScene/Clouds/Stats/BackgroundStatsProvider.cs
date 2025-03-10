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

        public async Task<IPrefabContainer<BackgroundImageStats>> GetStatsAsyncLevel(int levelNum)
        {
            IPrefabKey key = new LevelBackgroundImageStatsKey(levelNum);
            return await _prefabFetcher.GetPrefabAsync<BackgroundImageStats>(key);
        }

        public async Task<IPrefabContainer<BackgroundImageStats>> GetStatsAsyncSideQuest(int sideQuestID)
        {
            IPrefabKey key = new SideQuestBackgroundImageStatsKey(sideQuestID);
            return await _prefabFetcher.GetPrefabAsync<BackgroundImageStats>(key);
        }
    }
}