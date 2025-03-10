using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Utils.BattleScene.Lifetime
{
    public interface ILifetimeEventBroadcaster
    {
        IBroadcastingProperty<bool> IsPaused { get; }
    }
}