using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundStatsProvider : IBackgroundStatsProvider
    {
        public async Task<PrefabContainer<BackgroundImageStats>> GetStatsAsyncLevel(int levelNum)
        {
            IPrefabKey key = new LevelBackgroundImageStatsKey(levelNum);
            return await PrefabFetcher.GetPrefabAsync<BackgroundImageStats>(key);
        }

        public async Task<PrefabContainer<BackgroundImageStats>> GetStatsAsyncSideQuest(int sideQuestID)
        {
            IPrefabKey key = new SideQuestBackgroundImageStatsKey(sideQuestID);
            return await PrefabFetcher.GetPrefabAsync<BackgroundImageStats>(key);
        }
    }
}