using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
    public class AliveTargetFilter : ITargetFilter
    {
        public virtual bool IsMatch(ITarget target)
        {
            return !target.IsDestroyed;
        }
    }
}
