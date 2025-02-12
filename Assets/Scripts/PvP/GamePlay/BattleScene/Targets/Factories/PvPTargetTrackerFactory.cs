using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetTrackers;
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

        public ITargetTracker CreateTargetTracker(ITargetFinder targetFinder)
        {
            return new PvPTargetTracker(targetFinder);
        }

        public IPvPRankedTargetTracker CreateUserChosenInRangeTargetTracker(ITargetTracker inRangeTargetTracker)
        {
            return new PvPUserChosenInRangeTargetTracker(inRangeTargetTracker, UserChosenTargetTracker);
        }

        public IPvPRankedTargetTracker CreateRankedTargetTracker(ITargetFinder targetFinder, IPvPTargetRanker targetRanker)
        {
            return new PvPRankedTargetTracker(targetFinder, targetRanker);
        }

        public IPvPRankedTargetTracker CreateCompositeTracker(params IPvPRankedTargetTracker[] targetTrackers)
        {
            return new PvPCompositeTracker(targetTrackers);
        }
    }
}