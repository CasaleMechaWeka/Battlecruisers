using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;

namespace BattleCruisers.Targets.TargetProviders
{
    public class ShipBlockingEnemyProvider : ITargetProvider, ITargetConsumer
    {
        public ITarget Target { get; set; }

        public ShipBlockingEnemyProvider(ITargetsFactory targetsFactory, ITargetDetector enemyDetector, Faction enemyFaction)
        {
            // Enemy detection for stopping (ignore aircraft)
            IList<TargetType> blockingEnemyTypes = new List<TargetType>() { TargetType.Ships, TargetType.Cruiser, TargetType.Buildings };
            ITargetFilter enemyDetectionFilter = targetsFactory.CreateTargetFilter(enemyFaction, blockingEnemyTypes);
            ITargetFinder enemyFinder = targetsFactory.CreateRangedTargetFinder(enemyDetector, enemyDetectionFilter);

            ITargetRanker targetRanker = targetsFactory.CreateEqualTargetRanker();
            ITargetProcessor targetProcessor = targetsFactory.CreateTargetProcessor(enemyFinder, targetRanker);
            targetProcessor.AddTargetConsumer(this);
            targetProcessor.StartProcessingTargets();

            targetProcessor.Dispose();
        }
    }
}
