using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetFinderFactory : IPvPTargetFinderFactory
    {
        public IPvPTargetFinder CreateRangedTargetFinder(IPvPTargetDetector targetDetector, IPvPTargetFilter targetFilter)
        {
            return new PvPRangedTargetFinder(targetDetector, targetFilter);
        }

        public IPvPTargetFinder CreateMinRangeTargetFinder(IPvPTargetDetector maxRangeTargetDetector, IPvPTargetDetector minRangeTargetDetector, IPvPTargetFilter targetFilter)
        {
            return new PvPMinRangeTargetFinder(maxRangeTargetDetector, minRangeTargetDetector, targetFilter);
        }

        public IPvPTargetFinder CreateAttackingTargetFinder(IPvPDamagable parentDamagable, IPvPTargetFilter targetFilter)
        {
            return new PvPAttackingTargetFinder(parentDamagable, targetFilter);
        }
    }
}