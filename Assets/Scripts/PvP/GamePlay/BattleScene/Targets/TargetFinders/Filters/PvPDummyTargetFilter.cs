using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

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
    }
}
