using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
	public class DummyTargetFilter : ITargetFilter
	{
        private readonly bool _isMatchResult;

        public DummyTargetFilter(bool isMatchResult)
        {
            _isMatchResult = isMatchResult;
        }

		public virtual bool IsMatch(ITarget target)
		{
            return _isMatchResult;
		}
	}
}
