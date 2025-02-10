using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Lifetime
{
    public interface IPvPLifetimeEventBroadcaster
    {
        IBroadcastingProperty<bool> IsPaused { get; }
    }
}