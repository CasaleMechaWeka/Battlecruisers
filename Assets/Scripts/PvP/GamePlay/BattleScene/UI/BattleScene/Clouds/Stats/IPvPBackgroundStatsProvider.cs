using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats
{
    public interface IPvPBackgroundStatsProvider
    {
        Task<IPrefabContainer<PvPBackgroundImageStats>> GetStatsAsync(int levelNum);
    }
}