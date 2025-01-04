using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetFinderFactory
    {
        IPvPTargetFinder CreateRangedTargetFinder(IPvPTargetDetector targetDetector, ITargetFilter targetFilter);
        IPvPTargetFinder CreateMinRangeTargetFinder(IPvPTargetDetector maxRangeTargetDetector, IPvPTargetDetector minRangeTargetDetector, ITargetFilter targetFilter);
        IPvPTargetFinder CreateAttackingTargetFinder(IDamagable parentDamagable, ITargetFilter targetFilter);
    }
}