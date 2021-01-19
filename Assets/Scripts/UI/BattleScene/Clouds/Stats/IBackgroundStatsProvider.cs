using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public interface IBackgroundStatsProvider
    {
        Task<IPrefabContainer<BackgroundImageStats>> GetStatsAsync(int levelNum);
    }
}