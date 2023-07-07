using BattleCruisers.Data.Static.Strategies;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Helper
{
    public enum PvPStrategyType
    {
        Balanced, Rush, Boom
    }

    public interface IPvPStrategyFactory
    {
        IPvPStrategy GetAdaptiveStrategy();
        IPvPStrategy GetBasicStrategy();
    }
}