using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
	public class TargetProcessor : ITargetProcessor
	{
		// List of targets, in decreasing priority
		private readonly IList<ITarget> _targets;
		private readonly IList<ITargetConsumer> _targetConsumers;
		private ITargetFinder _targetFinder;
		private readonly ITargetRanker _targetRanker;

		private ITarget HighestPriorityTarget
		{
			get
			{
				return _targets.Count != 0 ? _targets[0] : null;
			}
		}

		public TargetProcessor(ITargetFinder targetFinder, ITargetRanker targetRanker)
		{
			_targets = new List<ITarget>();
			_targetConsumers =  new List<ITargetConsumer>();
			_targetFinder = targetFinder;
			_targetRanker = targetRanker;

			_targetFinder.TargetFound += TargetFinder_TargetFound;
			_targetFinder.TargetLost += TargetFinder_TargetLost;

			_targetFinder.StartFindingTargets();
		}

		private void TargetFinder_TargetFound(object sender, TargetEventArgs e)
		{
			Logging.Log(Tags.TARGET_PROCESSORS, "TargetFinder_TargetFound");
			Assert.IsFalse(_targets.Contains(e.Target));

			int insertionIndex = FindInsertionIndex(e.Target);

			_targets.Insert(insertionIndex, e.Target);

			if (System.Object.ReferenceEquals(e.Target, HighestPriorityTarget))
			{
				AssignTarget(HighestPriorityTarget);
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
			Logging.Log(Tags.TARGET_PROCESSORS, "TargetFinder_TargetLost");
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

		private void AssignTarget(ITarget target)
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
