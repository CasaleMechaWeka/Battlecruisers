using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

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

        public virtual bool IsMatch(ITarget target, VariantPrefab variant)
        {
            return _isMatchResult;
        }
    }
}
