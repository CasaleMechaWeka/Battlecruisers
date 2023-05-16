using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Lifetime
{
    public interface IPvPLifetimeEventBroadcaster
    {
        IPvPBroadcastingProperty<bool> IsPaused { get; }
    }
}