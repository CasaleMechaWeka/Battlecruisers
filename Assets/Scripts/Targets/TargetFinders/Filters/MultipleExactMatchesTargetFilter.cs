using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using System.Collections.Generic;

namespace BattleCruisers.Targets.TargetFinders.Filters
{
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

        public virtual bool IsMatch(ITarget target, VariantPrefab variant)
        {
            return _matches.Contains(target);
        }
    }
}
