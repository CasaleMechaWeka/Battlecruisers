using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats
{
    public interface IPvPBackgroundStatsProvider
    {
        Task<IPvPPrefabContainer<PvPBackgroundImageStats>> GetStatsAsync(int levelNum);
    }
}