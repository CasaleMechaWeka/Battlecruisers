using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters
{
    public class PvPMultipleExactMatchesTargetFilter : IPvPExactMatchTargetFilter
    {
        private readonly HashSet<IPvPTarget> _matches;

        public IPvPTarget Target
        {
            set
            {
                if (value != null)
                {
                    _matches.Add(value);
                }
            }
        }

        public PvPMultipleExactMatchesTargetFilter()
        {
            _matches = new HashSet<IPvPTarget>();
        }

        public virtual bool IsMatch(IPvPTarget target)
        {
            return _matches.Contains(target);
        }
    }
}
