using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
	// FELIX  Actually prioritise/sort targets :P
	public class TargetProcessor : ITargetProcessor
	{
		// List of targets, in decreasing priority
		private readonly IList<IFactionable> _targets;
		private readonly IList<ITargetConsumer> _targetConsumers;
		private ITargetFinder _targetFinder;

		private IFactionable HighestPriorityTarget
		{
			get
			{
				return _targets.Count != 0 ? _targets[0] : null;
			}
		}

		public TargetProcessor(ITargetFinder targetFinder)
		{
			_targets = new List<IFactionable>();
			_targetConsumers =  new List<ITargetConsumer>();
			_targetFinder = targetFinder;

			_targetFinder.TargetFound += TargetFinder_TargetFound;
			_targetFinder.TargetLost += TargetFinder_TargetLost;
		}

		// FELIX  Insert at right priority
		private void TargetFinder_TargetFound(object sender, TargetEventArgs e)
		{
			Assert.IsFalse(_targets.Contains(e.Target));
			_targets.Add(e.Target);

			if (System.Object.ReferenceEquals(e.Target, HighestPriorityTarget))
			{
				AssignTarget(HighestPriorityTarget);
			}
		}
		
		private void TargetFinder_TargetLost(object sender, TargetEventArgs e)
		{
			Assert.IsTrue(_targets.Contains(e.Target));
			bool wasHighestPriorityTarget = System.Object.ReferenceEquals(e.Target, HighestPriorityTarget);
			_targets.Remove(e.Target);

			if (wasHighestPriorityTarget)
			{
				AssignTarget(HighestPriorityTarget);
			}
		}

		public void AddTargetConsumer(ITargetConsumer targetConsumer)
		{
			if (_targetConsumers.Contains(targetConsumer))
			{
				throw new ArgumentException();
			}

			_targetConsumers.Add(targetConsumer);

			targetConsumer.Target = HighestPriorityTarget;
		}

		public void RemoveTargetConsumer(ITargetConsumer targetConsumer)
		{
			if (!_targetConsumers.Contains(targetConsumer))
			{
				throw new ArgumentException();
			}

			_targetConsumers.Remove(targetConsumer);
		}

		private void AssignTarget(IFactionable target)
		{
			foreach (ITargetConsumer consumer in _targetConsumers)
			{
				consumer.Target = target;
			}
		}

		public void Dispose()
		{
			_targetFinder.TargetFound -= TargetFinder_TargetFound;
			_targetFinder.TargetLost -= TargetFinder_TargetLost;
			_targetFinder = null;
		}
	}
}
