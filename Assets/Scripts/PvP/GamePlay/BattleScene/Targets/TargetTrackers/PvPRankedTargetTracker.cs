using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers
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
    public class PvPRankedTargetTracker : IPvPRankedTargetTracker
    {
        private readonly IPvPTargetFinder _targetFinder;
        private readonly IPvPTargetRanker _targetRanker;
        // List of targets, in decreasing priority
        private readonly IList<PvPRankedTarget> _targets;

        public PvPRankedTarget HighestPriorityTarget => _targets.FirstOrDefault();

        public event EventHandler HighestPriorityTargetChanged;

        public PvPRankedTargetTracker(IPvPTargetFinder targetFinder, IPvPTargetRanker targetRanker)
        {
            PvPHelper.AssertIsNotNull(targetFinder, targetRanker);

            _targetFinder = targetFinder;
            _targetRanker = targetRanker;
            _targets = new List<PvPRankedTarget>();

            _targetFinder.TargetFound += TargetFinder_TargetFound;
            _targetFinder.TargetLost += TargetFinder_TargetLost;
        }

        private void TargetFinder_TargetFound(object sender, PvPTargetEventArgs e)
        {
            // Logging.Verbose(Tags.RANKED_TARGET_TRACKER, $"_targetFinder: {_targetFinder}  Target found: {e.Target}");

            if (AreTrackingTarget(e.Target))
            {
                // Should never be the case but defensive programming because rarely it IS
                // the case :/
                // Logging.Warn(Tags.RANKED_TARGET_TRACKER, $"_targetFinder: {_targetFinder}  Received TargetFound event for a target that has already been found");
                return;
            }

            if (e.Target.IsDestroyed)
            {
                // Edge case, where collider object is destroyed and OnTriggerExit2D() 
                // is called **before** OnTriggerEnter2D().  Hence ignore the
                // TargetFound events for already destroyed objects, as they have
                // already had their corresponding TargetLost event.
                // Logging.Warn(Tags.RANKED_TARGET_TRACKER, $"_targetFinder: {_targetFinder}  Received TargetFound event for a destroyed target");
                return;
            }

            PvPRankedTarget newTarget = CreateRankedTarget(e.Target);
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

        private void TargetFinder_TargetLost(object sender, PvPTargetEventArgs e)
        {
            // Logging.Verbose(Tags.RANKED_TARGET_TRACKER, $"_targetFinder: {_targetFinder}  Target lost: {e.Target}");

            if (!AreTrackingTarget(e.Target))
            {
                // Edge case, where collider object is destroyed and OnTriggerExit2D() 
                // is called **before** OnTriggerEnter2D().  Hence ignore this
                // TargetLost event.
                // Logging.Warn(Tags.RANKED_TARGET_TRACKER, $"_targetFinder: {_targetFinder}  Received TargetLost event without a preceeding TargetFound event");
                return;
            }

            bool wasHighestPriorityTarget = ReferenceEquals(e.Target, HighestPriorityTarget.Target);
            PvPRankedTarget targetToRemove = CreateRankedTarget(e.Target);
            _targets.Remove(targetToRemove);

            if (wasHighestPriorityTarget)
            {
                InvokeHighestPriorityTargetChanged();
            }
        }

        private bool AreTrackingTarget(IPvPTarget target)
        {
            return _targets.Any(rankedTarget => ReferenceEquals(rankedTarget.Target, target));
        }

        private void InvokeHighestPriorityTargetChanged()
        {
            // Logging.Verbose(Tags.RANKED_TARGET_TRACKER, $"_targetFinder: {_targetFinder}");
            HighestPriorityTargetChanged?.Invoke(this, EventArgs.Empty);
        }

        private PvPRankedTarget CreateRankedTarget(IPvPTarget target)
        {
            int targetRank = _targetRanker.RankTarget(target);
            return new PvPRankedTarget(target, targetRank);
        }

        public void DisposeManagedState()
        {
            _targets.Clear();
            _targetFinder.TargetFound -= TargetFinder_TargetFound;
            _targetFinder.TargetLost -= TargetFinder_TargetLost;
        }
    }
}