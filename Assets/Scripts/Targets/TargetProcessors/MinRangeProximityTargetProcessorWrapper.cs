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
			CircleTargetDetectorController maxRangeDetector = transform.FindNamedComponent<CircleTargetDetectorController>("MaxRangeDetector");
            maxRangeDetector.Initialise(args.MaxRangeInM);
            maxRangeDetector.Initialise();

            CircleTargetDetectorController minRangeDetector = transform.FindNamedComponent<CircleTargetDetectorController>("MinRangeDetector");
            minRangeDetector.Initialise(args.MinRangeInM);
            minRangeDetector.Initialise();

            // Create target finder
            ITargetFilter enemyDetectionFilter = args.TargetFactories.FilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            return args.TargetFactories.FinderFactory.CreateMinRangeTargetFinder(maxRangeDetector, minRangeDetector, enemyDetectionFilter);
        }
    }
}
