using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static.Strategies.Helper;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models
{
    public interface IPvPSkirmishModel
    {
        bool WasRandomPlayerCruiser { get; }
        PvPHullKey PlayerCruiser { get; }
        bool WasRandomAICruiser { get; }
        PvPHullKey AICruiser { get; }
        bool WasRandomStrategy { get; }
        StrategyType AIStrategy { get; }
        Difficulty Difficulty { get; }
        int BackgroundLevelNum { get; }
    }
}