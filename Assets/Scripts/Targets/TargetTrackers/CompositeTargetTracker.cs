using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetTrackers
{
    public class CompositeTracker : IRankedTargetTracker
    {
        private readonly IRankedTargetTracker[] _targetTrackers;

        private const int MIN_NUM_OF_TARGET_TRACKERS = 2;

        public RankedTarget HighestPriorityTarget { get; private set; }

        public event EventHandler HighestPriorityTargetChanged;

        public CompositeTracker(params IRankedTargetTracker[] targetTrackers)
        {
            Assert.IsNotNull(targetTrackers);
            Assert.IsTrue(targetTrackers.Length >= MIN_NUM_OF_TARGET_TRACKERS);

            _targetTrackers = targetTrackers;

            foreach (IRankedTargetTracker targetTracker in _targetTrackers)
            {
                targetTracker.HighestPriorityTargetChanged += TargetTracker_HighestPriorityTargetChanged;
            }
        }

        private void TargetTracker_HighestPriorityTargetChanged(object sender, EventArgs e)
        {
            RankedTarget currentHighestRankedTarget = HighestPriorityTarget;
            HighestPriorityTarget = FindHighestRankedTarget();

            Logging.Verbose(Tags.COMPOSITE_TARGET_TRACKER, $"Current highest: {currentHighestRankedTarget}  New highest: {HighestPriorityTarget}");

            if (!ReferenceEquals(currentHighestRankedTarget, HighestPriorityTarget))
            {
                Logging.Verbose(Tags.COMPOSITE_TARGET_TRACKER, $"About to invoke HighestPriorityTargetChanged event :D");
                HighestPriorityTargetChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private RankedTarget FindHighestRankedTarget()
        {
            RankedTarget highestRankedTarget = null;
            int maxRankSoFar = int.MinValue;

            foreach (IRankedTargetTracker targetTracker in _targetTrackers)
            {
                RankedTarget rankedTarget = targetTracker.HighestPriorityTarget;

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
            foreach (IRankedTargetTracker targetTracker in _targetTrackers)
            {
                targetTracker.DisposeManagedState();
            }
        }
    }
}