using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
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

        public virtual bool IsMatch(IPvPTarget target)
        {
            return _isMatchResult;
        }

        public virtual bool IsMatch(IPvPTarget target, VariantPrefab variant)
        {
            return _isMatchResult;
        }
    }
}
