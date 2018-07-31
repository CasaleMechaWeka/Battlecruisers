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
		private readonly IHighestPriorityTargetTracker _highestPriorityTargetTracker;
		private readonly IList<ITargetConsumer> _targetConsumers;

		public TargetProcessor(IHighestPriorityTargetTracker highestPriorityTargetTracker)
		{
            _highestPriorityTargetTracker = highestPriorityTargetTracker;
			_targetConsumers =  new List<ITargetConsumer>();

            _highestPriorityTargetTracker.HighestPriorityTargetChanged += _highestPriorityTargetTracker_HighestPriorityTargetChanged;
        }

        private void _highestPriorityTargetTracker_HighestPriorityTargetChanged(object sender, EventArgs e)
        {
            // PERF
            // Copy list to avoid Add-/Remove- TargetConsumer during this method 
            // causing an enumerable modified while iterating exception (AntiAirBalancingTests)
            foreach (ITargetConsumer consumer in _targetConsumers.ToList())
            {
                consumer.Target = _highestPriorityTargetTracker.HighestPriorityTarget;
            }
        }

        public void StartProcessingTargets()
        {
			_highestPriorityTargetTracker.StartTrackingTargets();
        }

		public void AddTargetConsumer(ITargetConsumer targetConsumer)
		{
            Assert.IsFalse(_targetConsumers.Contains(targetConsumer));

			_targetConsumers.Add(targetConsumer);

			targetConsumer.Target = _highestPriorityTargetTracker.HighestPriorityTarget;
		}

		public void RemoveTargetConsumer(ITargetConsumer targetConsumer)
		{
            Assert.IsTrue(_targetConsumers.Contains(targetConsumer));

			_targetConsumers.Remove(targetConsumer);
		}

		public void DisposeManagedState()
		{
            _highestPriorityTargetTracker.HighestPriorityTargetChanged -= _highestPriorityTargetTracker_HighestPriorityTargetChanged;
		}
    }
}
