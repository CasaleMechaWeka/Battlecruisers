using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetTrackerFactory : IPvPTargetTrackerFactory
    {
        public IPvPRankedTargetTracker UserChosenTargetTracker { get; }

        public PvPTargetTrackerFactory(IPvPRankedTargetTracker userChosenTargetTracker)
        {
            Assert.IsNotNull(userChosenTargetTracker);
            UserChosenTargetTracker = userChosenTargetTracker;
        }

        public IPvPTargetTracker CreateTargetTracker(IPvPTargetFinder targetFinder)
        {
            return new PvPTargetTracker(targetFinder);
        }

        public IPvPRankedTargetTracker CreateUserChosenInRangeTargetTracker(IPvPTargetTracker inRangeTargetTracker)
        {
            return new PvPUserChosenInRangeTargetTracker(inRangeTargetTracker, UserChosenTargetTracker);
        }

        public IPvPRankedTargetTracker CreateRankedTargetTracker(IPvPTargetFinder targetFinder, IPvPTargetRanker targetRanker)
        {
            return new PvPRankedTargetTracker(targetFinder, targetRanker);
        }

        public IPvPRankedTargetTracker CreateCompositeTracker(params IPvPRankedTargetTracker[] targetTrackers)
        {
            return new PvPCompositeTracker(targetTrackers);
        }
    }
}