using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    /// <summary>
    /// Assigns the highest priority target to target consumers.
    /// </summary>
    public class TargetProcessor : ITargetProcessor
	{
		private readonly IRankedTargetTracker _rankedTargetTracker;
		private readonly IList<ITargetConsumer> _targetConsumers;

        private ITarget HighestPriorityTarget
        {
            get
            {
                ITarget highestPriorityTarget = _rankedTargetTracker.HighestPriorityTarget?.Target;
                Logging.Log(Tags.TARGET_PROCESSORS, highestPriorityTarget.ToString());
                return highestPriorityTarget;
            }
        }

		public TargetProcessor(IRankedTargetTracker rankedTargetTracker)
		{
            _rankedTargetTracker = rankedTargetTracker;
			_targetConsumers =  new List<ITargetConsumer>();

            _rankedTargetTracker.HighestPriorityTargetChanged += _rankedTargetTracker_HighestPriorityTargetChanged;
        }

        private void _rankedTargetTracker_HighestPriorityTargetChanged(object sender, EventArgs e)
        {
            Logging.Log(Tags.TARGET_PROCESSORS, $"Updating {_targetConsumers.Count} target consumers :)");

            // PERF  Copying list to avoid modification while enumeration exception :P
            // Copy list to avoid Add-/Remove- TargetConsumer during this method 
            // causing an enumerable modified while iterating exception (AntiAirBalancingTests)
            foreach (ITargetConsumer consumer in _targetConsumers.ToList())
            {
                consumer.Target = HighestPriorityTarget;
            }
        }

		public void AddTargetConsumer(ITargetConsumer targetConsumer)
		{
            Assert.IsFalse(_targetConsumers.Contains(targetConsumer));

			_targetConsumers.Add(targetConsumer);

			targetConsumer.Target = HighestPriorityTarget;
		}

		public void RemoveTargetConsumer(ITargetConsumer targetConsumer)
		{
            Assert.IsTrue(_targetConsumers.Contains(targetConsumer));

			_targetConsumers.Remove(targetConsumer);
		}

		public void DisposeManagedState()
		{
            _rankedTargetTracker.HighestPriorityTargetChanged -= _rankedTargetTracker_HighestPriorityTargetChanged;
		}
    }
}
