using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetFinderFactory
    {
        IPvPTargetFinder CreateRangedTargetFinder(IPvPTargetDetector targetDetector, IPvPTargetFilter targetFilter);
        IPvPTargetFinder CreateMinRangeTargetFinder(IPvPTargetDetector maxRangeTargetDetector, IPvPTargetDetector minRangeTargetDetector, ITargetFilter targetFilter);
        IPvPTargetFinder CreateAttackingTargetFinder(IPvPDamagable parentDamagable, IPvPTargetFilter targetFilter);
    }
}