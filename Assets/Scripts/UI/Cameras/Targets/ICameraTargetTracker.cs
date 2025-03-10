using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.Cameras.Targets
{
    public interface ICameraTargetTracker
    {
        IBroadcastingProperty<bool> IsOnTarget { get; }
    }
}