using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Targets.TargetProcessors
{
    public class HighestPriorityTargetTrackerTests
    {
        private IHighestPriorityTargetTracker _targetTracker;
        private ITargetFinder _targetFinder;
		private ITargetRanker _targetRanker;
		private ITarget _target1, _target2, _target3;
        private int _highestPriorityTargetChangedCount;

		[SetUp]
		public void TestSetup()
		{
			_targetFinder = Substitute.For<ITargetFinder>();
			_targetRanker = Substitute.For<ITargetRanker>();

			_target1 = Substitute.For<ITarget>();
			_target2 = Substitute.For<ITarget>();
			_target3 = Substitute.For<ITarget>();

			_targetTracker = new HighestPriorityTargetTracker(_targetFinder, _targetRanker);

            _targetTracker.StartTrackingTargets();
            _targetFinder.Received().StartFindingTargets();

            _highestPriorityTargetChangedCount = 0;
            _targetTracker.HighestPriorityTargetChanged += (sender, e) => _highestPriorityTargetChangedCount++;

			UnityAsserts.Assert.raiseExceptions = true;
		}

		[Test]
		public void InitialState()
		{
            Assert.IsNull(_targetTracker.HighestPriorityTarget);
		}

        #region TargetFound
        [Test]
		public void TargetFound_FirstTarget_IsHighestPriority()
		{
			_targetRanker.RankTarget(_target1).Returns(50);
			InvokeTargetFound(_target1);

            _targetRanker.Received().RankTarget(_target1);
            Assert.AreSame(_target1, _targetTracker.HighestPriorityTarget);
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);
		}

        [Test]
        public void TargetFound_AlreadyHaveFoundTarget_IsIgnored()
        {
            _targetRanker.RankTarget(_target1).Returns(50);

            // First time target is found => All good
            InvokeTargetFound(_target1);
            _targetRanker.Received().RankTarget(_target1);
            _targetRanker.ClearReceivedCalls();

            // Second time target is found => Ignore
            InvokeTargetFound(_target1);
            _targetRanker.DidNotReceive().RankTarget(_target1);
        }

        [Test]
        public void TargetFound_TargetIsDestroyed_IsIgnored()
        {
            _target1.IsDestroyed.Returns(true);
            InvokeTargetFound(_target1);

            _targetRanker.DidNotReceive().RankTarget(_target1);
            Assert.IsNull(_targetTracker.HighestPriorityTarget);
            Assert.AreEqual(0, _highestPriorityTargetChangedCount);
        }

        [Test]
        public void TargetFound_MultipleTargets_RankedCorrectly()
        {
            _targetRanker.RankTarget(_target1).Returns(25);
            _targetRanker.RankTarget(_target2).Returns(15);
            _targetRanker.RankTarget(_target3).Returns(50);

            // First target => Highest ranked
            InvokeTargetFound(_target1);

            _targetRanker.Received().RankTarget(_target1);
            Assert.AreSame(_target1, _targetTracker.HighestPriorityTarget);
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);

            // Second target => Not highest ranked
            InvokeTargetFound(_target2);

            _targetRanker.Received().RankTarget(_target2);
            Assert.AreSame(_target1, _targetTracker.HighestPriorityTarget);
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);

            // Third target => Highest ranked
            InvokeTargetFound(_target3);

            _targetRanker.Received().RankTarget(_target3);
            Assert.AreSame(_target3, _targetTracker.HighestPriorityTarget);
            Assert.AreEqual(2, _highestPriorityTargetChangedCount);
        }
        #endregion TargetFound

        #region TargetLost
        [Test]
        public void TargetLost_TargetWasNeverFound_Ignores()
        {
            InvokeTargetLost(_target1);
        }

        [Test]
        public void TargetLost_WasHighestPriorityTarget_EmitsEvent()
        {
            // Find target
            _targetRanker.RankTarget(_target1).Returns(50);
            InvokeTargetFound(_target1);
            Assert.AreSame(_target1, _targetTracker.HighestPriorityTarget);
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);

            // Lose target
            InvokeTargetLost(_target1);
            Assert.IsNull(_targetTracker.HighestPriorityTarget);
            Assert.AreEqual(2, _highestPriorityTargetChangedCount);
        }

        [Test]
        public void TargetLost_WasNotHighestPriorityTarget_DoesNotEmitEvent()
        {
            // Find target
            _targetRanker.RankTarget(_target1).Returns(25);
            InvokeTargetFound(_target1);
            Assert.AreSame(_target1, _targetTracker.HighestPriorityTarget);
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);

            // Find higher priority target
            _targetRanker.RankTarget(_target2).Returns(50);
            InvokeTargetFound(_target2);
            Assert.AreSame(_target2, _targetTracker.HighestPriorityTarget);
            Assert.AreEqual(2, _highestPriorityTargetChangedCount);

            // Lose lower priority target
            InvokeTargetLost(_target1);
            Assert.AreSame(_target2, _targetTracker.HighestPriorityTarget);
            Assert.AreEqual(2, _highestPriorityTargetChangedCount);
        }
        #endregion TargetLost

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
