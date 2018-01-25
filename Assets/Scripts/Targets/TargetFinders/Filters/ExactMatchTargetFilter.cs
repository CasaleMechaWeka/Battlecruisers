using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
	public class ExactMatchTargetFilter : IExactMatchTargetFilter
	{
		public ITarget Target { private get; set; }

		public virtual bool IsMatch(ITarget target)
		{
			return ReferenceEquals(Target, target);
		}
	}
}
