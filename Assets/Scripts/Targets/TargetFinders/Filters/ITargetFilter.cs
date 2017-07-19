using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
    public interface ITargetFilter
	{
		bool IsMatch(ITarget target);
	}
}
