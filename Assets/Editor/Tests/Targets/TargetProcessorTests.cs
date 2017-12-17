using BattleCruisers.Buildables;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Targets
{
    public class TargetProcessorTests 
	{
		private ITargetProcessor _targetProcessor;
		private ITargetFinder _targetFinder;
		private ITargetConsumer _targetConsumer;
		private ITargetRanker _targetRanker;
		private ITarget _target1, _target2, _target3;

		[SetUp]
		public void TestSetup()
		{
			_targetFinder = Substitute.For<ITargetFinder>();
			_targetConsumer = Substitute.For<ITargetConsumer>();
			_targetRanker = Substitute.For<ITargetRanker>();

			_target1 = Substitute.For<ITarget>();
			_target2 = Substitute.For<ITarget>();
			_target3 = Substitute.For<ITarget>();

			_targetProcessor = new TargetProcessor(_targetFinder, _targetRanker);

            _targetProcessor.StartProcessingTargets();
            _targetFinder.Received().StartFindingTargets();

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
			_targetRanker.RankTarget(_target1).Returns(50);
			InvokeTargetFound(_target1);

			_targetRanker.RankTarget(_target2).Returns(100);
			InvokeTargetFound(_target2);

			_targetRanker.RankTarget(_target3).Returns(75);
			InvokeTargetFound(_target3);

			_targetProcessor.AddTargetConsumer(_targetConsumer);
			_targetConsumer.Received().Target = _target2;
		}

		[Test]
		public void FirstTarget_AssignsToExistingConsumers()
		{
			_targetProcessor.AddTargetConsumer(_targetConsumer);

			_targetRanker.RankTarget(_target1).Returns(50);
			InvokeTargetFound(_target1);

			_targetConsumer.Received().Target = _target1;
		}

        [Test]
        public void TargetFound_TargetIsDestroyed_IsIgnored()
        {
            _target1.IsDestroyed.Returns(true);
            InvokeTargetFound(_target1);
            _targetRanker.DidNotReceive().RankTarget(_target1);
        }

		[Test]
		public void TargetLost_RemovesTargetFromConsumers()
		{
			FirstTarget_AssignsToExistingConsumers();

			InvokeTargetLost(_target1);
			_targetConsumer.Received().Target = null;
		}

		[Test]
		public void TargetLost_MovesToNextHighestPriorityTarget()
		{
			ExistingTargets_AssignsHighestPriority();

			InvokeTargetLost(_target2);
			_targetConsumer.Received().Target = _target3;
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
		public void DoubleFindSameTarget_OnlyAssignsFirstTime()
		{
            _targetProcessor.AddTargetConsumer(_targetConsumer);

			InvokeTargetFound(_target1);
            _targetConsumer.Received().Target = _target1;

            _targetConsumer.ClearReceivedCalls();

			InvokeTargetFound(_target1);
            _targetConsumer.DidNotReceive().Target = _target1;
		}

		[Test]
		public void DoubleAddSameConsumer_Throws()
		{
			_targetProcessor.AddTargetConsumer(_targetConsumer);
			Assert.Throws<UnityAsserts.AssertionException>(() => _targetProcessor.AddTargetConsumer(_targetConsumer));
		}

		[Test]
		public void RemoveUnaddedConsumer_Throws()
		{
			Assert.Throws<UnityAsserts.AssertionException>(() => _targetProcessor.RemoveTargetConsumer(_targetConsumer));
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
