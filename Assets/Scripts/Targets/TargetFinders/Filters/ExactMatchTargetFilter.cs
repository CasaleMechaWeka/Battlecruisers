using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
    public interface IExactMatchTargetFilter : ITargetFilter, ITargetConsumer { }

	public class ExactMatchTargetFilter : IExactMatchTargetFilter
	{
		public ITarget Target { private get; set; }

		public virtual bool IsMatch(ITarget target)
		{
			return object.ReferenceEquals(Target, target);
		}
	}
}
