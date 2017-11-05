using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;
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
        }

        public void StartProcessingTargets()
        {
			_targetFinder.StartFindingTargets();
        }

		private void TargetFinder_TargetFound(object sender, TargetEventArgs e)
		{
			Logging.Log(Tags.TARGET_PROCESSORS, _targetFinder, "TargetFinder_TargetFound");
			Assert.IsFalse(_targets.Contains(e.Target));

            if (e.Target.IsDestroyed)
            {
                // Edge case, where collider object is destroyed and OnTriggerExit2D() 
                // is called **before** OnTriggerEnter2D().  Hence ignore the
                // TargetFound events for already destroyed objects, as they have
                // already had their corresponding TargetLost event.
                Logging.Warn(Tags.TARGET_PROCESSORS, "Received TargetFound event for a destroyed target");
				return;
            }

			int insertionIndex = FindInsertionIndex(e.Target);

			_targets.Insert(insertionIndex, e.Target);

			if (ReferenceEquals(e.Target, HighestPriorityTarget))
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
			Logging.Log(Tags.TARGET_PROCESSORS, _targetFinder, "TargetFinder_TargetLost");

            if (!_targets.Contains(e.Target))
            {
				// Edge case, where collider object is destroyed and OnTriggerExit2D() 
				// is called **before** OnTriggerEnter2D().  Hence ignore this
				// TargetLost event.
				Logging.Warn(Tags.TARGET_PROCESSORS, "Received TargetLost event without a preceeding TargetFound event");
				return;
			}

			bool wasHighestPriorityTarget = ReferenceEquals(e.Target, HighestPriorityTarget);
			_targets.Remove(e.Target);

			if (wasHighestPriorityTarget)
			{
				AssignTarget(HighestPriorityTarget);
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
