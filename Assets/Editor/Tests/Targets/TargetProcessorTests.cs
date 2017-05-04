using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using BattleCruisers.Buildables;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Targets
{
	public class TargetProcessorTests 
	{
		private ITargetProcessor _targetProcessor;
		private ITargetFinder _targetFinder;
		private ITargetConsumer _targetConsumer;
		private IFactionable _target1, _target2;

		[SetUp]
		public void TestSetup()
		{
			_targetFinder = Substitute.For<ITargetFinder>();
			_targetConsumer = Substitute.For<ITargetConsumer>();
			_target1 = Substitute.For<IFactionable>();
			_target2 = Substitute.For<IFactionable>();

			_targetProcessor = new TargetProcessor(_targetFinder);
			_targetFinder.Received(1).StartFindingTargets();
		}

		[Test]
		public void NoTargets_AssignsNull()
		{
			_targetProcessor.AddTargetConsumer(_targetConsumer);
			_targetConsumer.Received().Target = null;
		}

		[Test]
		public void ExistingTargets_AssignsHighestPriority()
		{
			// Highest priority is currently simply the first added target
			InvokeTargetFound(_target1);
			InvokeTargetFound(_target2);

			_targetProcessor.AddTargetConsumer(_targetConsumer);
			_targetConsumer.Received().Target = _target1;
		}


		[Test]
		public void FirstTarget_AssignsToExistingConsumers()
		{
			_targetProcessor.AddTargetConsumer(_targetConsumer);

			InvokeTargetFound(_target1);
			_targetConsumer.Received().Target = _target1;
		}

		[Test]
		public void TargetLost_RemovesTargetFromConsumers()
		{
			FirstTarget_AssignsToExistingConsumers();

			InvokeTargetLost(_target1);
			_targetConsumer.Received().Target = null;
		}

		// FELIX  Test exceptions

		private void InvokeTargetFound(IFactionable target)
		{
			_targetFinder.TargetFound += Raise.EventWith(_targetFinder, new TargetEventArgs(target));
		}

		private void InvokeTargetLost(IFactionable target)
		{
			_targetFinder.TargetLost += Raise.EventWith(_targetFinder, new TargetEventArgs(target));
		}
	}
}
