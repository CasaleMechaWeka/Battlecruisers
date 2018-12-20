using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class MinRangeProximityTargetProcessorWrapper : ProximityTargetProcessorWrapper
    {
        protected override ITargetFinder CreateTargetFinder(ITargetProcessorArgs args)
        {
			CircleTargetDetector maxRangeDetector = transform.FindNamedComponent<CircleTargetDetector>("MaxRangeDetector");
            maxRangeDetector.Initialise(args.MaxRangeInM);

            CircleTargetDetector minRangeDetector = transform.FindNamedComponent<CircleTargetDetector>("MinRangeDetector");
            minRangeDetector.Initialise(args.MinRangeInM);

            // Create target finder
            ITargetFilter enemyDetectionFilter = args.TargetsFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            return args.TargetsFactory.CreateMinRangeTargetFinder(maxRangeDetector, minRangeDetector, enemyDetectionFilter);
        }
    }
}
