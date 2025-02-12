using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetFinderFactory
    {
        ITargetFinder CreateRangedTargetFinder(ITargetDetector targetDetector, ITargetFilter targetFilter);
        ITargetFinder CreateMinRangeTargetFinder(ITargetDetector maxRangeTargetDetector, ITargetDetector minRangeTargetDetector, ITargetFilter targetFilter);
        ITargetFinder CreateAttackingTargetFinder(IDamagable parentDamagable, ITargetFilter targetFilter);
    }
}