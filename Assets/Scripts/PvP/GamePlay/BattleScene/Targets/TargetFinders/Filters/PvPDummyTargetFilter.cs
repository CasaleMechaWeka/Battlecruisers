using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters
{
    public class PvPDummyTargetFilter : IPvPTargetFilter
    {
        private readonly bool _isMatchResult;

        public PvPDummyTargetFilter(bool isMatchResult)
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
