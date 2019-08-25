using BattleCruisers.Buildables;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    public class EnemyShipBlockerInitialiser : MonoBehaviour
    {
        public ITargetTracker Initialise(
            ITargetFactoriesProvider targetFactoriesProvider, 
            ITargetTrackerFactory targetTrackerFactory,
            Faction enemyFaction)
        {
            Helper.AssertIsNotNull(targetFactoriesProvider, targetTrackerFactory);

            TargetDetectorController targetDetectorController = GetComponent<TargetDetectorController>();
            Assert.IsNotNull(targetDetectorController);
            targetDetectorController.Initialise();

            IList<TargetType> targetTypesToFind = new List<TargetType>()
            {
                TargetType.Ships
            };

            ITargetFinder targetFinder
                = targetFactoriesProvider.FinderFactory.CreateRangedTargetFinder(
                    targetDetectorController,
                    targetFactoriesProvider.FilterFactory.CreateTargetFilter(enemyFaction, targetTypesToFind));

            return targetTrackerFactory.CreateTargetTracker(targetFinder);
        }
    }
}