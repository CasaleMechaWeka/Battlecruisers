using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders.Filters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetFinderFactory : IPvPTargetFinderFactory
    {
        public IPvPTargetFinder CreateRangedTargetFinder(ITargetDetector targetDetector, ITargetFilter targetFilter)
        {
            return new PvPRangedTargetFinder(targetDetector, targetFilter);
        }

        public IPvPTargetFinder CreateMinRangeTargetFinder(ITargetDetector maxRangeTargetDetector, ITargetDetector minRangeTargetDetector, ITargetFilter targetFilter)
        {
            return new PvPMinRangeTargetFinder(maxRangeTargetDetector, minRangeTargetDetector, targetFilter);
        }

        public IPvPTargetFinder CreateAttackingTargetFinder(IDamagable parentDamagable, ITargetFilter targetFilter)
        {
            return new PvPAttackingTargetFinder(parentDamagable, targetFilter);
        }
    }
}