using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;

namespace BattleCruisers.Targets.Factories
{
    public class TargetFinderFactory : ITargetFinderFactory
    {
        public ITargetFinder CreateRangedTargetFinder(ITargetDetector targetDetector, ITargetFilter targetFilter)
        {
            return new RangedTargetFinder(targetDetector, targetFilter);
        }

        public ITargetFinder CreateMinRangeTargetFinder(ITargetDetector maxRangeTargetDetector, ITargetDetector minRangeTargetDetector, ITargetFilter targetFilter)
        {
            return new MinRangeTargetFinder(maxRangeTargetDetector, minRangeTargetDetector, targetFilter);
        }

        public ITargetFinder CreateAttackingTargetFinder(IDamagable parentDamagable, ITargetFilter targetFilter)
        {
            return new AttackingTargetFinder(parentDamagable, targetFilter);
        }
    }
}