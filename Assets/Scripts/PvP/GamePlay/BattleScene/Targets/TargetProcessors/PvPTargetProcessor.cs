using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    /// <summary>
    /// Assigns the highest priority target to target consumers.
    /// </summary>
    public class PvPTargetProcessor : IPvPTargetProcessor
    {
        private readonly IPvPRankedTargetTracker _rankedTargetTracker;
        private readonly IList<IPvPTargetConsumer> _targetConsumers;

        private IPvPTarget HighestPriorityTarget
        {
            get
            {
                IPvPTarget highestPriorityTarget = _rankedTargetTracker.HighestPriorityTarget?.Target;
                // Logging.Verbose(Tags.TARGET_PROCESSORS, $"{highestPriorityTarget}  position: {highestPriorityTarget?.Position}");
                return highestPriorityTarget;
            }
        }

        public PvPTargetProcessor(IPvPRankedTargetTracker rankedTargetTracker)
        {
            _rankedTargetTracker = rankedTargetTracker;
            _targetConsumers = new List<IPvPTargetConsumer>();

            _rankedTargetTracker.HighestPriorityTargetChanged += _rankedTargetTracker_HighestPriorityTargetChanged;
        }

        private void _rankedTargetTracker_HighestPriorityTargetChanged(object sender, EventArgs e)
        {
            // Logging.Verbose(Tags.TARGET_PROCESSORS, $"Updating {_targetConsumers.Count} target consumers :)");

            // PERF  Copying list to avoid modification while enumeration exception :P
            // Copy list to avoid Add-/Remove- TargetConsumer during this method 
            // causing an enumerable modified while iterating exception (AntiAirBalancingTests)
            foreach (IPvPTargetConsumer consumer in _targetConsumers.ToList())
            {
                consumer.Target = HighestPriorityTarget;
            }
        }

        public void AddTargetConsumer(IPvPTargetConsumer targetConsumer)
        {
            Assert.IsFalse(_targetConsumers.Contains(targetConsumer));

            _targetConsumers.Add(targetConsumer);

            targetConsumer.Target = HighestPriorityTarget;
        }

        public void RemoveTargetConsumer(IPvPTargetConsumer targetConsumer)
        {
            Assert.IsTrue(_targetConsumers.Contains(targetConsumer));

            _targetConsumers.Remove(targetConsumer);
        }

        public void DisposeManagedState()
        {
            // Logging.LogMethod(Tags.TARGET_PROCESSORS);
            _rankedTargetTracker.HighestPriorityTargetChanged -= _rankedTargetTracker_HighestPriorityTargetChanged;
        }
    }
}
