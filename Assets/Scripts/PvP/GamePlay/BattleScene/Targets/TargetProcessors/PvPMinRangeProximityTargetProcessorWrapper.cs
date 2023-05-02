using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public class PvPMinRangeProximityTargetProcessorWrapper : PvPProximityTargetProcessorWrapper
    {
        protected override IPvPTargetFinder CreateTargetFinder(IPvPTargetProcessorArgs args)
        {
            PvPCircleTargetDetectorController maxRangeDetector = transform.FindNamedComponent<PvPCircleTargetDetectorController>("MaxRangeDetector");
            maxRangeDetector.Initialise(args.MaxRangeInM);

            PvPCircleTargetDetectorController minRangeDetector = transform.FindNamedComponent<PvPCircleTargetDetectorController>("MinRangeDetector");
            minRangeDetector.Initialise(args.MinRangeInM);

            // Create target finder
            IPvPTargetFilter enemyDetectionFilter = args.TargetFactories.FilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            //Debug.Log(args.EnemyFaction);
            return args.TargetFactories.FinderFactory.CreateMinRangeTargetFinder(maxRangeDetector, minRangeDetector, enemyDetectionFilter);
        }
    }
}
