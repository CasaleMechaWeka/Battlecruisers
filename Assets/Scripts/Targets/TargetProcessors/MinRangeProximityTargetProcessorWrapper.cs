using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class MinRangeProximityTargetProcessorWrapper : ProximityTargetProcessorWrapper
    {
        protected override ITargetFinder CreateTargetFinder(ITargetProcessorArgs args)
        {
			CircleTargetDetectorController maxRangeDetector = transform.FindNamedComponent<CircleTargetDetectorController>("MaxRangeDetector");
            maxRangeDetector.Initialise(args.MaxRangeInM);

            CircleTargetDetectorController minRangeDetector = transform.FindNamedComponent<CircleTargetDetectorController>("MinRangeDetector");
            minRangeDetector.Initialise(args.MinRangeInM);

            // Create target finder
            ITargetFilter enemyDetectionFilter = args.TargetFactories.FilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            //Debug.Log(args.EnemyFaction);
            return args.TargetFactories.FinderFactory.CreateMinRangeTargetFinder(maxRangeDetector, minRangeDetector, enemyDetectionFilter);
        }
    }
}
