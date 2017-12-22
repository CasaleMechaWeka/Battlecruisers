using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProviders
{
    public class ShipBlockingEnemyProvider : ITargetProvider, ITargetConsumer
    {
        private readonly ITargetFilter _targetValidator;

        private ITarget _target;
        public ITarget Target 
        { 
            get { return _target; }
            set
            {
                _target = value;

                if (_target != null)
                {
                    Assert.IsTrue(_targetValidator.IsMatch(_target));
				}
            }
        }

        public ShipBlockingEnemyProvider(ITargetsFactory targetsFactory, ITargetDetector enemyDetector, Faction enemyFaction, ITargetFilter targetValidator)
        {
            Helper.AssertIsNotNull(targetsFactory, enemyDetector, targetValidator);

            _targetValidator = targetValidator;

            IList<TargetType> blockingEnemyTypes = new List<TargetType>() { TargetType.Ships, TargetType.Cruiser, TargetType.Buildings };
            ITargetFilter enemyDetectionFilter = targetsFactory.CreateTargetFilter(enemyFaction, blockingEnemyTypes);
            ITargetFinder enemyFinder = targetsFactory.CreateRangedTargetFinder(enemyDetector, enemyDetectionFilter);

            ITargetRanker targetRanker = targetsFactory.CreateEqualTargetRanker();
            ITargetProcessor targetProcessor = targetsFactory.CreateTargetProcessor(enemyFinder, targetRanker);
            targetProcessor.AddTargetConsumer(this);
            targetProcessor.StartProcessingTargets();
        }
    }
}
