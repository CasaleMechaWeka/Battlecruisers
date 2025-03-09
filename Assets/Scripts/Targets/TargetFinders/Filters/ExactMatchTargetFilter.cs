using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
	public class ExactMatchTargetFilter : IExactMatchTargetFilter
	{
		public ITarget Target { private get; set; }

		public virtual bool IsMatch(ITarget target)
		{
			return ReferenceEquals(Target, target);
		}

		public virtual bool IsMatch(ITarget target, VariantPrefab variant)
		{
			return ReferenceEquals(Target, target);
		}
	}
}
