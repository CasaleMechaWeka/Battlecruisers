using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPEnemyShipBlockerInitialiser : MonoBehaviour
    {
        public IPvPTargetTracker Initialise(
            IPvPTargetFactoriesProvider targetFactoriesProvider,
            IPvPTargetTrackerFactory targetTrackerFactory,
            PvPFaction enemyFaction)
        {
            Helper.AssertIsNotNull(targetFactoriesProvider, targetTrackerFactory);

            PvPTargetDetectorController targetDetectorController = GetComponent<PvPTargetDetectorController>();
            Assert.IsNotNull(targetDetectorController);

            IList<PvPTargetType> targetTypesToFind = new List<PvPTargetType>()
            {
                PvPTargetType.Ships
            };

            IPvPTargetFinder targetFinder
                = targetFactoriesProvider.FinderFactory.CreateRangedTargetFinder(
                    targetDetectorController,
                    targetFactoriesProvider.FilterFactory.CreateTargetFilter(enemyFaction, targetTypesToFind));

            return targetTrackerFactory.CreateTargetTracker(targetFinder);
        }
    }
}