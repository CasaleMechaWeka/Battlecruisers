using System.Collections.Generic;
using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
    public class FactionAndTargetTypeFilter : FactionTargetFilter
	{
        private readonly IList<TargetType> _targetTypes;

        public FactionAndTargetTypeFilter(Faction factionToDetect, IList<TargetType> targetTypes)
            : base(factionToDetect)
		{
			_targetTypes = targetTypes;
		}

		public override bool IsMatch(ITarget target)
		{
			return 
                base.IsMatch(target)
				&& _targetTypes.Contains(target.TargetType);
		}
	}
}
