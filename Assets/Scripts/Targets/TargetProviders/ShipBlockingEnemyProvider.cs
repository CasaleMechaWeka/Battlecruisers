using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProviders
{
    public class ShipBlockingEnemyProvider : BroadcastingTargetProvider, ITargetConsumer
    {
        private readonly ITargetFilter _isInFrontFilter;

        ITarget ITargetConsumer.Target
        {
            set
            {
                if (value != null)
                {
                    Assert.IsTrue(_isInFrontFilter.IsMatch(value));
                }

                Target = value;
            }
        }

        public ShipBlockingEnemyProvider(ITargetsFactory targetsFactory, ITargetDetector enemyDetector, IUnit parentUnit)
        {
            Helper.AssertIsNotNull(targetsFactory, enemyDetector, parentUnit);

            _isInFrontFilter = targetsFactory.CreateTargetInFrontFilter(parentUnit);

            IList<TargetType> blockingEnemyTypes = new List<TargetType>() { TargetType.Ships, TargetType.Cruiser, TargetType.Buildings };
            Faction enemyFaction = Helper.GetOppositeFaction(parentUnit.Faction);
            ITargetFilter enemyDetectionFilter = targetsFactory.CreateTargetFilter(enemyFaction, blockingEnemyTypes);
            ITargetFinder enemyFinder = targetsFactory.CreateRangedTargetFinder(enemyDetector, enemyDetectionFilter);

            ITargetRanker targetRanker = targetsFactory.CreateEqualTargetRanker();
            ITargetProcessor targetProcessor = targetsFactory.CreateTargetProcessor(enemyFinder, targetRanker);
            targetProcessor.AddTargetConsumer(this);
            targetProcessor.StartProcessingTargets();
        }
    }
}
