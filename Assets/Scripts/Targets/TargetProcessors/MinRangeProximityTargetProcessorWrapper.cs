using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetProcessors
{
    // FELIX Extend ProximityTargetProcessorWrapper :)
    public class MinRangeProximityTargetProcessorWrapper : TargetProcessorWrapper
    {
        private ITargetFinder _targetFinder;

        protected override ITargetProcessor CreateTargetProcessor(ITargetProcessorArgs args, ITargetRanker targetRanker)
        {
			CircleTargetDetector maxRangeDetector = transform.FindNamedComponent<CircleTargetDetector>("MaxRangeDetector");
            maxRangeDetector.Initialise(args.MaxRangeInM);

            CircleTargetDetector minRangeDetector = transform.FindNamedComponent<CircleTargetDetector>("MinRangeDetector");
            minRangeDetector.Initialise(args.MinRangeInM);

            // Create target finder
            ITargetFilter enemyDetectionFilter = args.TargetsFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            _targetFinder = args.TargetsFactory.CreateMinRangeTargetFinder(maxRangeDetector, minRangeDetector, enemyDetectionFilter);

            // Start processing targets
            return args.TargetsFactory.CreateTargetProcessor(_targetFinder, targetRanker);
        }

        protected override void CleanUp()
        {
            base.CleanUp();
            _targetFinder.Dispose();
        }
    }
}
