using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders.Filters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetFinderFactory
    {
        IPvPTargetFinder CreateRangedTargetFinder(ITargetDetector targetDetector, ITargetFilter targetFilter);
        IPvPTargetFinder CreateMinRangeTargetFinder(ITargetDetector maxRangeTargetDetector, ITargetDetector minRangeTargetDetector, ITargetFilter targetFilter);
        IPvPTargetFinder CreateAttackingTargetFinder(IDamagable parentDamagable, ITargetFilter targetFilter);
    }
}