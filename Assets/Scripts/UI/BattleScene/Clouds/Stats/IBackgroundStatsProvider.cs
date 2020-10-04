using System.Threading.Tasks;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public interface IBackgroundStatsProvider
    {
        Task<IBackgroundImageStats> GetStatsAsync(int levelNum);
    }
}