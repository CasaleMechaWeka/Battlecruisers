using BattleCruisers.Targets.TargetTrackers.Ranking;
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

            if (!ReferenceEquals(currentHighestRankedTarget, HighestPriorityTarget))
            {
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