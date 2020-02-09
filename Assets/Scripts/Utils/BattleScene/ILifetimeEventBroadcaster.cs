using UnityCommon.Properties;

namespace BattleCruisers.Utils.BattleScene
{
    public interface ILifetimeEventBroadcaster
    {
        IBroadcastingProperty<bool> IsPaused { get; }
    }
}