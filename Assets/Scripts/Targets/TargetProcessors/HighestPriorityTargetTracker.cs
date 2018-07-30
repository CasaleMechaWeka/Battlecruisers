using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.Targets.TargetProcessors
{
    // FELIX  Test :P
    // FELIX  Keep track of rank, so composite target tracker can compare ranks.
    // class RankedTarget
    // + Target
    // + Rank
    public class HighestPriorityTargetTracker : IHighestPriorityTargetTracker
    {
        private readonly ITargetFinder _targetFinder;
        private readonly ITargetRanker _targetRanker;
        // List of targets, in decreasing priority
        private readonly IList<ITarget> _targets;

        public ITarget HighestPriorityTarget { get { return _targets.FirstOrDefault(); } }

        public event EventHandler HighestPriorityTargetChanged;

        public HighestPriorityTargetTracker(ITargetFinder targetFinder, ITargetRanker targetRanker)
        {
            Helper.AssertIsNotNull(targetFinder, targetRanker);

            _targetFinder = targetFinder;
            _targetRanker = targetRanker;
			_targets = new List<ITarget>();

            _targetFinder.TargetFound += TargetFinder_TargetFound;
            _targetFinder.TargetLost += TargetFinder_TargetLost;
        }

        private void TargetFinder_TargetFound(object sender, TargetEventArgs e)
        {
            Logging.Log(Tags.HIGHEST_PRIORITY_TARGET_TRACKER, _targetFinder, "TargetFinder_TargetFound(): " + e.Target);

            if (_targets.Contains(e.Target))
            {
                // Should never be the case but defensive programming because rarely it IS
                // the case :/
                Logging.Warn(Tags.HIGHEST_PRIORITY_TARGET_TRACKER, "Received TargetFound event for a target that has already been found");
                return;
            }

            if (e.Target.IsDestroyed)
            {
                // Edge case, where collider object is destroyed and OnTriggerExit2D() 
                // is called **before** OnTriggerEnter2D().  Hence ignore the
                // TargetFound events for already destroyed objects, as they have
                // already had their corresponding TargetLost event.
                Logging.Warn(Tags.HIGHEST_PRIORITY_TARGET_TRACKER, "Received TargetFound event for a destroyed target");
                return;
            }

            int insertionIndex = FindInsertionIndex(e.Target);

            _targets.Insert(insertionIndex, e.Target);

            if (ReferenceEquals(e.Target, HighestPriorityTarget))
            {
                InvokeHighestPriorityTargetChanged();
            }
        }

        private int FindInsertionIndex(ITarget target)
        {
            int insertionIndex = _targets.Count;
            int newTargetRank = _targetRanker.RankTarget(target);

            for (int i = 0; i < _targets.Count; ++i)
            {
                int existingTargetRank = _targetRanker.RankTarget(_targets[i]);

                if (newTargetRank > existingTargetRank)
                {
                    insertionIndex = i;
                    break;
                }
            }

            return insertionIndex;
        }

        private void TargetFinder_TargetLost(object sender, TargetEventArgs e)
        {
            Logging.Log(Tags.HIGHEST_PRIORITY_TARGET_TRACKER, _targetFinder, "TargetFinder_TargetLost(): " + e.Target);

            if (!_targets.Contains(e.Target))
            {
                // Edge case, where collider object is destroyed and OnTriggerExit2D() 
                // is called **before** OnTriggerEnter2D().  Hence ignore this
                // TargetLost event.
                Logging.Warn(Tags.HIGHEST_PRIORITY_TARGET_TRACKER, "Received TargetLost event without a preceeding TargetFound event");
                return;
            }

            bool wasHighestPriorityTarget = ReferenceEquals(e.Target, HighestPriorityTarget);
            _targets.Remove(e.Target);

            if (wasHighestPriorityTarget)
            {
                InvokeHighestPriorityTargetChanged();
            }
        }

        private void InvokeHighestPriorityTargetChanged()
        {
            if (HighestPriorityTargetChanged != null)
            {
                HighestPriorityTargetChanged.Invoke(this, EventArgs.Empty);
            }
        }

        public void StartTrackingTargets()
        {
            _targetFinder.StartFindingTargets();
        }

        public void DisposeManagedState()
        {
            _targetFinder.TargetFound -= TargetFinder_TargetFound;
            _targetFinder.TargetLost -= TargetFinder_TargetLost;
        }
    }
}