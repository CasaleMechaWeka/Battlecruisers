using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Utils;

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
			bool result 
                = base.IsMatch(target)
				    && _targetTypes.Contains(target.TargetType);
            Logging.Log(Tags.TARGET_FILTER, $"result: {result}  targetType: {target.TargetType}");
            return result;
        }
    }
}
