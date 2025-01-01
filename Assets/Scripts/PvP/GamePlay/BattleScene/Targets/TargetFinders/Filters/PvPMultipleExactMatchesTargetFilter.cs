using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters
{
    public class PvPMultipleExactMatchesTargetFilter : IPvPExactMatchTargetFilter
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

        public PvPMultipleExactMatchesTargetFilter()
        {
            _matches = new HashSet<ITarget>();
        }

        public virtual bool IsMatch(ITarget target)
        {
            return _matches.Contains(target);
        }

        public virtual bool IsMatch(ITarget target, VariantPrefab variant)
        {
            return _matches.Contains(target);
        }
    }
}
