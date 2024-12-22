using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers
{
    public class PvPCompositeTracker : IPvPRankedTargetTracker
    {
        private readonly IPvPRankedTargetTracker[] _targetTrackers;

        private const int MIN_NUM_OF_TARGET_TRACKERS = 2;

        public PvPRankedTarget HighestPriorityTarget { get; private set; }

        public event EventHandler HighestPriorityTargetChanged;

        public PvPCompositeTracker(params IPvPRankedTargetTracker[] targetTrackers)
        {
            Assert.IsNotNull(targetTrackers);
            Assert.IsTrue(targetTrackers.Length >= MIN_NUM_OF_TARGET_TRACKERS);

            _targetTrackers = targetTrackers;

            foreach (IPvPRankedTargetTracker targetTracker in _targetTrackers)
            {
                targetTracker.HighestPriorityTargetChanged += TargetTracker_HighestPriorityTargetChanged;
            }
        }

        private void TargetTracker_HighestPriorityTargetChanged(object sender, EventArgs e)
        {
            PvPRankedTarget currentHighestRankedTarget = HighestPriorityTarget;
            HighestPriorityTarget = FindHighestRankedTarget();

            // Logging.Verbose(Tags.COMPOSITE_TARGET_TRACKER, $"Current highest: {currentHighestRankedTarget}  New highest: {HighestPriorityTarget}");

            if (!ReferenceEquals(currentHighestRankedTarget, HighestPriorityTarget))
            {
                // Logging.Verbose(Tags.COMPOSITE_TARGET_TRACKER, $"About to invoke HighestPriorityTargetChanged event :D");
                HighestPriorityTargetChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private PvPRankedTarget FindHighestRankedTarget()
        {
            PvPRankedTarget highestRankedTarget = null;
            int maxRankSoFar = int.MinValue;

            foreach (IPvPRankedTargetTracker targetTracker in _targetTrackers)
            {
                PvPRankedTarget rankedTarget = targetTracker.HighestPriorityTarget;

                if (rankedTarget != null
                    && rankedTarget.Rank > maxRankSoFar)
                {
                    highestRankedTarget = rankedTarget;
                    maxRankSoFar = rankedTarget.Rank;
                }
            }

            return highestRankedTarget;
        }

        public void DisposeManagedState()
        {
            foreach (IPvPRankedTargetTracker targetTracker in _targetTrackers)
            {
                targetTracker.DisposeManagedState();
            }
        }
    }
}