using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class MinRangeProximityTargetProcessorWrapper : ProximityTargetProcessorWrapper
    {
        protected override ITargetFinder CreateTargetFinder(TargetProcessorArgs args)
        {
            CircleTargetDetectorController maxRangeDetector = transform.FindNamedComponent<CircleTargetDetectorController>("MaxRangeDetector");
            maxRangeDetector.Initialise(args.MaxRangeInM);

            CircleTargetDetectorController minRangeDetector = transform.FindNamedComponent<CircleTargetDetectorController>("MinRangeDetector");
            minRangeDetector.Initialise(args.MinRangeInM);

            // Create target finder
            ITargetFilter enemyDetectionFilter = new FactionAndTargetTypeFilter(args.EnemyFaction, args.AttackCapabilities);
            //Debug.Log(args.EnemyFaction);
            return new MinRangeTargetFinder(maxRangeDetector, minRangeDetector, enemyDetectionFilter);
        }
    }
}
