using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Cruisers.Construction
{
    public interface IPopulationLimitMonitor
    {
        IBroadcastingProperty<bool> IsPopulationLimitReached { get; }
    }
}