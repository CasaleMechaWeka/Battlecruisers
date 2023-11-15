using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction
{
    public interface IPvPPopulationLimitMonitor
    {
        IPvPBroadcastingProperty<bool> IsPopulationLimitReached { get; }
    }
}