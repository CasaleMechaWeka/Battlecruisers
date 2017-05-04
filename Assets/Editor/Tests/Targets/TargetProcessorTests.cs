using BattleCruisers.Buildables;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using System;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Targets
{
	public class TargetProcessorTests 
	{
		private ITargetProcessor _targetProcessor;
		private ITargetFinder _targetFinder;
		private ITargetConsumer _targetConsumer;
		private ITarget _target1, _target2;

		[SetUp]
		public void TestSetup()
		{
			_targetFinder = Substitute.For<ITargetFinder>();
			_targetConsumer = Substitute.For<ITargetConsumer>();
			_target1 = Substitute.For<ITarget>();
			_target2 = Substitute.For<ITarget>();

			_targetProcessor = new TargetProcessor(_targetFinder);
			_targetFinder.Received(1).StartFindingTargets();

			UnityAsserts.Assert.raiseExceptions = true;
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

		[Test]
		public void TargetLost_MovesToNextTarget()
		{
			ExistingTargets_AssignsHighestPriority();

			InvokeTargetLost(_target1);
			_targetConsumer.Received().Target = _target2;
		}

		[Test]
		public void RemovedConsumer_NoLongerReceivesTargets()
		{
			_targetProcessor.AddTargetConsumer(_targetConsumer);
			_targetProcessor.RemoveTargetConsumer(_targetConsumer);

			InvokeTargetFound(_target1);

			_targetConsumer.Received().Target = null;
			_targetConsumer.DidNotReceive().Target = _target1;
		}

		[Test]
		[ExpectedException(typeof(UnityAsserts.AssertionException))]
		public void DoubleFindSameTarget_Throws()
		{
			InvokeTargetFound(_target1);
			InvokeTargetFound(_target1);
		}

		[Test]
		[ExpectedException(typeof(UnityAsserts.AssertionException))]
		public void LostNotAddedTarget_Throws()
		{
			InvokeTargetLost(_target1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void DoubleAddSameConsumer_Throws()
		{
			_targetProcessor.AddTargetConsumer(_targetConsumer);
			_targetProcessor.AddTargetConsumer(_targetConsumer);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void RemoveUnaddedConsumer_Throws()
		{
			_targetProcessor.RemoveTargetConsumer(_targetConsumer);
		}

		private void InvokeTargetFound(ITarget target)
		{
			_targetFinder.TargetFound += Raise.EventWith(_targetFinder, new TargetEventArgs(target));
		}

		private void InvokeTargetLost(ITarget target)
		{
			_targetFinder.TargetLost += Raise.EventWith(_targetFinder, new TargetEventArgs(target));
		}
	}
}
