using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
    public class FactionAndTargetTypeFilter : ITargetFilter
	{
		private readonly Faction _factionToDetect;
		private readonly bool _ignoreDestroyedTargets;
        private readonly IList<TargetType> _targetTypes;

		public FactionAndTargetTypeFilter(Faction faction, IList<TargetType> targetTypes, bool ignoreDestroyedTargets)
		{
			_factionToDetect = faction;
            _ignoreDestroyedTargets = ignoreDestroyedTargets;
			_targetTypes = targetTypes;
		}

		public virtual bool IsMatch(ITarget target)
		{
			Logging.Log(Tags.TARGET_FILTER, string.Format("target.Faction: {0}  _factionToDetect: {1}  target.TargetType: {2}", target.Faction, _factionToDetect, target.TargetType));
            return (!_ignoreDestroyedTargets || !target.IsDestroyed)
                && target.Faction == _factionToDetect
				&& _targetTypes.Contains(target.TargetType);
		}
	}
}
