using BattleCruisers.Buildables;
using System.Collections.Generic;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
    // FELIX  Test :)
	public class MultipleExactMatchesTargetFilter : IExactMatchTargetFilter
	{
        private readonly HashSet<ITarget> _matches;

		public ITarget Target
        {
            set
            {
                if (value != null)
                {
                    _matches.Add(value);
                }
            }
        }

        public MultipleExactMatchesTargetFilter()
        {
            _matches = new HashSet<ITarget>();
        }

		public virtual bool IsMatch(ITarget target)
		{
            return _matches.Contains(target);
		}
	}
}
