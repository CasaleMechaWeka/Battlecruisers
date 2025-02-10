using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction
{
    public interface IPvPPopulationLimitMonitor
    {
        IBroadcastingProperty<bool> IsPopulationLimitReached { get; }
    }
}