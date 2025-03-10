using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public class PvPMinRangeProximityTargetProcessorWrapper : PvPProximityTargetProcessorWrapper
    {
        protected override ITargetFinder CreateTargetFinder(IPvPTargetProcessorArgs args)
        {
            PvPCircleTargetDetectorController maxRangeDetector = transform.FindNamedComponent<PvPCircleTargetDetectorController>("MaxRangeDetector");
            maxRangeDetector.Initialise(args.MaxRangeInM);

            PvPCircleTargetDetectorController minRangeDetector = transform.FindNamedComponent<PvPCircleTargetDetectorController>("MinRangeDetector");
            minRangeDetector.Initialise(args.MinRangeInM);

            // Create target finder
            ITargetFilter enemyDetectionFilter = args.TargetFactories.FilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            //Debug.Log(args.EnemyFaction);
            return args.TargetFactories.FinderFactory.CreateMinRangeTargetFinder(maxRangeDetector, minRangeDetector, enemyDetectionFilter);
        }
    }
}
