using BattleCruisers.Targets.TargetProcessors.Ranking;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetTrackers
{
    public class CompositeTracker : IHighestPriorityTargetTracker
    {
        private readonly IHighestPriorityTargetTracker[] _targetTrackers;

        private const int MIN_NUM_OF_TARGET_TRACKERS = 2;

        public RankedTarget HighestPriorityTarget { get; private set; }

        public event EventHandler HighestPriorityTargetChanged;

        public CompositeTracker(params IHighestPriorityTargetTracker[] targetTrackers)
        {
            Assert.IsNotNull(targetTrackers);
            Assert.IsTrue(targetTrackers.Length >= MIN_NUM_OF_TARGET_TRACKERS);

            _targetTrackers = targetTrackers;

            foreach (IHighestPriorityTargetTracker targetTracker in _targetTrackers)
            {
                targetTracker.HighestPriorityTargetChanged += TargetTracker_HighestPriorityTargetChanged;
            }
        }

        private void TargetTracker_HighestPriorityTargetChanged(object sender, EventArgs e)
        {
            HighestPriorityTarget = FindHighestRankedTarget();

            if (HighestPriorityTargetChanged != null)
            {
                HighestPriorityTargetChanged.Invoke(this, EventArgs.Empty);
            }
        }

        private RankedTarget FindHighestRankedTarget()
        {
            RankedTarget highestRankedTarget = null;
            int maxRankSoFar = int.MinValue;

            foreach (IHighestPriorityTargetTracker targetTracker in _targetTrackers)
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

        public void StartTrackingTargets()
        {
            foreach (IHighestPriorityTargetTracker targetTracker in _targetTrackers)
            {
                targetTracker.StartTrackingTargets();
            }
        }

        public void DisposeManagedState()
        {
            foreach (IHighestPriorityTargetTracker targetTracker in _targetTrackers)
            {
                targetTracker.DisposeManagedState();
            }
        }
    }
}