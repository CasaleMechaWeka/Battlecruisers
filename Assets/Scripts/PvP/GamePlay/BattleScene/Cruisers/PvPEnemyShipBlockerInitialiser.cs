using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPEnemyShipBlockerInitialiser : MonoBehaviour
    {
        public TargetTracker Initialise(
            TargetTrackerFactory targetTrackerFactory,
            Faction enemyFaction)
        {
            Helper.AssertIsNotNull(targetTrackerFactory);

            PvPTargetDetectorController targetDetectorController = GetComponent<PvPTargetDetectorController>();
            Assert.IsNotNull(targetDetectorController);

            IList<TargetType> targetTypesToFind = new List<TargetType>()
            {
                TargetType.Ships
            };

            ITargetFinder targetFinder
                = new RangedTargetFinder(
                    targetDetectorController,
                    new FactionAndTargetTypeFilter(enemyFaction, targetTypesToFind));

            return targetTrackerFactory.CreateTargetTracker(targetFinder);
        }
    }
}