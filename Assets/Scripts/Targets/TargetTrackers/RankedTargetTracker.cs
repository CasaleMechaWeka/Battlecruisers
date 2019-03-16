using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.Targets.TargetTrackers
{
    /// <summary>
    /// Keeps track of all targets found by the ITargetFinder, exposing the highest
    /// ranked target at any time.
    /// 
    /// Keeps a list of targets sorted in decreasing priority (ie, first target has
    /// the highest priority).
    /// 
    /// NOTE:
    /// + Assumes target rank value is constant.
    /// </summary>
    public class RankedTargetTracker : IRankedTargetTracker
    {
        private readonly ITargetFinder _targetFinder;
        private readonly ITargetRanker _targetRanker;
        // List of targets, in decreasing priority
        private readonly IList<RankedTarget> _targets;

        public RankedTarget HighestPriorityTarget => _targets.FirstOrDefault();

        public event EventHandler HighestPriorityTargetChanged;

        public RankedTargetTracker(ITargetFinder targetFinder, ITargetRanker targetRanker)
        {
            Helper.AssertIsNotNull(targetFinder, targetRanker);

            _targetFinder = targetFinder;
            _targetRanker = targetRanker;
            _targets = new List<RankedTarget>();

            _targetFinder.TargetFound += TargetFinder_TargetFound;
            _targetFinder.TargetLost += TargetFinder_TargetLost;
        }

        private void TargetFinder_TargetFound(object sender, TargetEventArgs e)
        {
            Logging.Log(Tags.RANKED_TARGET_TRACKER, e.Target.ToString());

            if (AreTrackingTarget(e.Target))
            {
                // Should never be the case but defensive programming because rarely it IS
                // the case :/
                Logging.Warn(Tags.RANKED_TARGET_TRACKER, "Received TargetFound event for a target that has already been found");
                return;
            }

            if (e.Target.IsDestroyed)
            {
                // Edge case, where collider object is destroyed and OnTriggerExit2D() 
                // is called **before** OnTriggerEnter2D().  Hence ignore the
                // TargetFound events for already destroyed objects, as they have
                // already had their corresponding TargetLost event.
                Logging.Warn(Tags.RANKED_TARGET_TRACKER, "Received TargetFound event for a destroyed target");
                return;
            }

            RankedTarget newTarget = CreateRankedTarget(e.Target);
            int insertionIndex = FindInsertionIndex(newTarget.Rank);

            _targets.Insert(insertionIndex, newTarget);

            if (ReferenceEquals(newTarget, HighestPriorityTarget))
            {
                InvokeHighestPriorityTargetChanged();
            }
        }

        private int FindInsertionIndex(int newTargetRank)
        {
            int insertionIndex = _targets.Count;

            for (int i = 0; i < _targets.Count; ++i)
            {
                if (newTargetRank > _targets[i].Rank)
                {
                    insertionIndex = i;
                    break;
                }
            }

            return insertionIndex;
        }

        private void TargetFinder_TargetLost(object sender, TargetEventArgs e)
        {
            Logging.Log(Tags.RANKED_TARGET_TRACKER, e.Target.ToString());

            if (!AreTrackingTarget(e.Target))
            {
                // Edge case, where collider object is destroyed and OnTriggerExit2D() 
                // is called **before** OnTriggerEnter2D().  Hence ignore this
                // TargetLost event.
                Logging.Warn(Tags.RANKED_TARGET_TRACKER, "Received TargetLost event without a preceeding TargetFound event");
                return;
            }

            bool wasHighestPriorityTarget = ReferenceEquals(e.Target, HighestPriorityTarget.Target);
            RankedTarget targetToRemove = CreateRankedTarget(e.Target);
            _targets.Remove(targetToRemove);

            if (wasHighestPriorityTarget)
            {
                InvokeHighestPriorityTargetChanged();
            }
        }

        private bool AreTrackingTarget(ITarget target)
        {
            return _targets.Any(rankedTarget => ReferenceEquals(rankedTarget.Target, target));
        }

        private void InvokeHighestPriorityTargetChanged()
        {
            HighestPriorityTargetChanged?.Invoke(this, EventArgs.Empty);
        }

        private RankedTarget CreateRankedTarget(ITarget target)
        {
            int targetRank = _targetRanker.RankTarget(target);
            return new RankedTarget(target, targetRank);
        }

        public void DisposeManagedState()
        {
            _targetFinder.TargetFound -= TargetFinder_TargetFound;
            _targetFinder.TargetLost -= TargetFinder_TargetLost;
        }
    }
}