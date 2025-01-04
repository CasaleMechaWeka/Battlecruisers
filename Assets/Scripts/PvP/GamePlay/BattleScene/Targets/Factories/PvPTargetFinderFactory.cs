using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetFinderFactory : IPvPTargetFinderFactory
    {
        public IPvPTargetFinder CreateRangedTargetFinder(IPvPTargetDetector targetDetector, ITargetFilter targetFilter)
        {
            return new PvPRangedTargetFinder(targetDetector, targetFilter);
        }

        public IPvPTargetFinder CreateMinRangeTargetFinder(IPvPTargetDetector maxRangeTargetDetector, IPvPTargetDetector minRangeTargetDetector, ITargetFilter targetFilter)
        {
            return new PvPMinRangeTargetFinder(maxRangeTargetDetector, minRangeTargetDetector, targetFilter);
        }

        public IPvPTargetFinder CreateAttackingTargetFinder(IDamagable parentDamagable, ITargetFilter targetFilter)
        {
            return new PvPAttackingTargetFinder(parentDamagable, targetFilter);
        }
    }
}