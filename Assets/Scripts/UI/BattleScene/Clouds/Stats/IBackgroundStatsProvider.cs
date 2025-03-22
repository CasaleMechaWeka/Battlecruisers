using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public interface IBackgroundStatsProvider
    {
        Task<PrefabContainer<BackgroundImageStats>> GetStatsAsyncLevel(int levelNum);
        Task<PrefabContainer<BackgroundImageStats>> GetStatsAsyncSideQuest(int sideQuestID);
    }
}