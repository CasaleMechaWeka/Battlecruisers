using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetTrackerFactory : ITargetTrackerFactory
    {
        public IRankedTargetTracker UserChosenTargetTracker { get; }

        public PvPTargetTrackerFactory(IRankedTargetTracker userChosenTargetTracker)
        {
            Assert.IsNotNull(userChosenTargetTracker);
            UserChosenTargetTracker = userChosenTargetTracker;
        }

        public ITargetTracker CreateTargetTracker(ITargetFinder targetFinder)
        {
            return new PvPTargetTracker(targetFinder);
        }

        public IRankedTargetTracker CreateUserChosenInRangeTargetTracker(ITargetTracker inRangeTargetTracker)
        {
            return new PvPUserChosenInRangeTargetTracker(inRangeTargetTracker, UserChosenTargetTracker);
        }

        public IRankedTargetTracker CreateRankedTargetTracker(ITargetFinder targetFinder, ITargetRanker targetRanker)
        {
            return new PvPRankedTargetTracker(targetFinder, targetRanker);
        }

        public IRankedTargetTracker CreateCompositeTracker(params IRankedTargetTracker[] targetTrackers)
        {
            return new PvPCompositeTracker(targetTrackers);
        }
    }
}