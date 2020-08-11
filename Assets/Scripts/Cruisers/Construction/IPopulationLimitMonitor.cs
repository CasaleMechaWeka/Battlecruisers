using UnityCommon.Properties;

namespace BattleCruisers.Cruisers.Construction
{
    public interface IPopulationLimitMonitor
    {
        IBroadcastingProperty<bool> IsPopulationLimitReached { get; }
    }
}