using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
    /// <summary>
    /// Only detects targets that have their IsDetectable property set to true.
    /// </summary>
    public class DetectableFilter : FactionAndTargetTypeFilter
	{
		private readonly bool _isDetectable;

		public DetectableFilter(Faction faction, bool isDetectable, params TargetType[] targetTypes)
			: base(faction, targetTypes)
		{
			_isDetectable = isDetectable;
		}

		public override bool IsMatch(ITarget target)
		{
			return base.IsMatch(target) && target.IsDetectable == _isDetectable;
		}
	}
}
