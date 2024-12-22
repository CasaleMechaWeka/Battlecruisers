using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers
{
    /// <summary>
    /// Keeps track of all targets found by the given ITargetFinder.
    /// </summary>
    public class PvPTargetTracker : IPvPTargetTracker
    {
        private readonly IPvPTargetFinder _targetFinder;
        private readonly IList<IPvPTarget> _targets;

        public event EventHandler TargetsChanged;

        public PvPTargetTracker(IPvPTargetFinder targetFinder)
        {
            Assert.IsNotNull(targetFinder);

            _targetFinder = targetFinder;
            _targets = new List<IPvPTarget>();

            _targetFinder.TargetFound += _targetFinder_TargetFound;
            _targetFinder.TargetLost += _targetFinder_TargetLost;
        }

        private void _targetFinder_TargetFound(object sender, PvPTargetEventArgs e)
        {
            // Logging.Log(Tags.TARGET_TRACKER, e.Target.ToString());

            // Should always be the case but defensive programming because rarely it is 
            // NOT the case :/
            if (!_targets.Contains(e.Target))
            {
                _targets.Add(e.Target);
                EmitTargetsChangedEvent();
            }
        }

        private void _targetFinder_TargetLost(object sender, PvPTargetEventArgs e)
        {
            // Logging.Log(Tags.TARGET_TRACKER, e.Target.ToString());

            // Should always be the case but defensive programming because rarely it is 
            // NOT the case :/
            if (_targets.Contains(e.Target))
            {
                _targets.Remove(e.Target);
                EmitTargetsChangedEvent();
            }
        }

        private void EmitTargetsChangedEvent()
        {
            TargetsChanged?.Invoke(this, EventArgs.Empty);
        }

        // PERF  Hashset instead of list?
        public bool ContainsTarget(IPvPTarget target)
        {
            bool result = _targets.Contains(target);
            // Logging.Log(Tags.TARGET_TRACKER, $"result: {result}  _targets.Count: {_targets.Count}");
            return result;
        }

        public void DisposeManagedState()
        {
            _targetFinder.TargetFound -= _targetFinder_TargetFound;
            _targetFinder.TargetLost -= _targetFinder_TargetLost;
            _targets.Clear();
            EmitTargetsChangedEvent();
        }
    }
}
