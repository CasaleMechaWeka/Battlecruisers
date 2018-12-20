using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Targets.TargetTrackers
{
    public class RankedTargetTrackerTests
    {
        private IRankedTargetTracker _targetTracker;
        private ITargetFinder _targetFinder;
		private ITargetRanker _targetRanker;
        private RankedTarget _highRankTarget, _mediumRankTarget, _lowRankTarget;
        private int _highestPriorityTargetChangedCount;

		[SetUp]
		public void TestSetup()
		{
			_targetFinder = Substitute.For<ITargetFinder>();
			_targetRanker = Substitute.For<ITargetRanker>();

			ITarget target1 = Substitute.For<ITarget>();
            _highRankTarget = new RankedTarget(target1, 3);
            _targetRanker.RankTarget(target1).Returns(3);

            ITarget target2 = Substitute.For<ITarget>();
            _mediumRankTarget = new RankedTarget(target2, 2);
            _targetRanker.RankTarget(target2).Returns(2);

            ITarget target3 = Substitute.For<ITarget>();
            _lowRankTarget = new RankedTarget(target3, 1);
            _targetRanker.RankTarget(target3).Returns(1);

            _targetTracker = new RankedTargetTracker(_targetFinder, _targetRanker);

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
			InvokeTargetFound(_mediumRankTarget.Target);

            _targetRanker.Received().RankTarget(_mediumRankTarget.Target);
            Assert.AreEqual(_mediumRankTarget, _targetTracker.HighestPriorityTarget);
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);
		}

        [Test]
        public void TargetFound_AlreadyHaveFoundTarget_IsIgnored()
        {
            // First time target is found => All good
            InvokeTargetFound(_mediumRankTarget.Target);
            _targetRanker.Received().RankTarget(_mediumRankTarget.Target);
            _targetRanker.ClearReceivedCalls();

            // Second time target is found => Ignore
            InvokeTargetFound(_mediumRankTarget.Target);
            _targetRanker.DidNotReceive().RankTarget(_mediumRankTarget.Target);
        }

        [Test]
        public void TargetFound_TargetIsDestroyed_IsIgnored()
        {
            _mediumRankTarget.Target.IsDestroyed.Returns(true);
            InvokeTargetFound(_mediumRankTarget.Target);

            _targetRanker.DidNotReceive().RankTarget(_mediumRankTarget.Target);
            Assert.IsNull(_targetTracker.HighestPriorityTarget);
            Assert.AreEqual(0, _highestPriorityTargetChangedCount);
        }

        [Test]
        public void TargetFound_MultipleTargets_RankedCorrectly()
        {
            // First target => Currently highest ranked
            InvokeTargetFound(_mediumRankTarget.Target);

            _targetRanker.Received().RankTarget(_mediumRankTarget.Target);
            Assert.AreEqual(_mediumRankTarget, _targetTracker.HighestPriorityTarget);
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);

            // Second target => Not highest ranked
            InvokeTargetFound(_lowRankTarget.Target);

            _targetRanker.Received().RankTarget(_lowRankTarget.Target);
            Assert.AreEqual(_mediumRankTarget, _targetTracker.HighestPriorityTarget);
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);

            // Third target => Highest ranked
            InvokeTargetFound(_highRankTarget.Target);

            _targetRanker.Received().RankTarget(_highRankTarget.Target);
            Assert.AreEqual(_highRankTarget, _targetTracker.HighestPriorityTarget);
            Assert.AreEqual(2, _highestPriorityTargetChangedCount);
        }
        #endregion TargetFound

        #region TargetLost
        [Test]
        public void TargetLost_TargetWasNeverFound_Ignores()
        {
            InvokeTargetLost(_mediumRankTarget.Target);
        }

        [Test]
        public void TargetLost_WasHighestPriorityTarget_EmitsEvent()
        {
            // Find target
            InvokeTargetFound(_mediumRankTarget.Target);
            Assert.AreEqual(_mediumRankTarget, _targetTracker.HighestPriorityTarget);
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);

            // Lose target
            InvokeTargetLost(_mediumRankTarget.Target);
            Assert.IsNull(_targetTracker.HighestPriorityTarget);
            Assert.AreEqual(2, _highestPriorityTargetChangedCount);
        }

        [Test]
        public void TargetLost_WasNotHighestPriorityTarget_DoesNotEmitEvent()
        {
            // Find target
            InvokeTargetFound(_lowRankTarget.Target);
            Assert.AreEqual(_lowRankTarget, _targetTracker.HighestPriorityTarget);
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);

            // Find higher priority target
            InvokeTargetFound(_highRankTarget.Target);
            Assert.AreEqual(_highRankTarget, _targetTracker.HighestPriorityTarget);
            Assert.AreEqual(2, _highestPriorityTargetChangedCount);

            // Lose lower priority target
            InvokeTargetLost(_lowRankTarget.Target);
            Assert.AreEqual(_highRankTarget, _targetTracker.HighestPriorityTarget);
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
