using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.Helpers
{
    public interface ITargetRangeHelper
    {
        bool IsTargetInRange(ITarget target);
    }
}
