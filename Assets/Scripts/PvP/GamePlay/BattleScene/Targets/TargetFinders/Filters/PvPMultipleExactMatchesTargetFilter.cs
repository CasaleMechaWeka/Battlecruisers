using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
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

        public virtual bool IsMatch(IPvPTarget target, VariantPrefab variant)
        {
            return _matches.Contains(target);
        }
    }
}
