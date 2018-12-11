using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Targets.TargetTrackers.UserChosen
{
    public class UserChosenTargetManagerTests
    {
        private IUserChosenTargetManager _targetManager;
        private ITarget _target1, _target2;
        private RankedTarget _rankedTarget1, _rankedTarget2;
        private int _highestPriorityTargetChangedCount;

        private const int USER_CHOSEN_TARGET_RANK = int.MaxValue;

        [SetUp]
        public void TestSetup()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _targetManager = new UserChosenTargetManager();

            _target1 = Substitute.For<ITarget>();
            _rankedTarget1 = new RankedTarget(_target1, USER_CHOSEN_TARGET_RANK);

            _target2 = Substitute.For<ITarget>();
            _rankedTarget2 = new RankedTarget(_target2, USER_CHOSEN_TARGET_RANK);

            _highestPriorityTargetChangedCount = 0;

            _targetManager.HighestPriorityTargetChanged += (sender, e) => _highestPriorityTargetChangedCount++;
        }

        [Test]
        public void InitialState()
        {
            Assert.IsNull(_targetManager.HighestPriorityTarget);
        }

        [Test]
        public void SetFirstTarget_UpdatesHighestPriorityTarget_EmitsEvent()
        {
            _targetManager.Target = _target1;

            Assert.AreEqual(1, _highestPriorityTargetChangedCount);
            Assert.AreEqual(_rankedTarget1, _targetManager.HighestPriorityTarget);
        }

        [Test]
        public void SetTargetToNull_UpdatesHighestPriorityTarget_EmitsEvent()
        {
            _targetManager.Target = _target1;
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);

            _targetManager.Target = null;
            Assert.AreEqual(2, _highestPriorityTargetChangedCount);
            Assert.IsNull(_targetManager.HighestPriorityTarget);
        }

        [Test]
        public void SetSecondSameTarget_DoesNotEmitEvent()
        {
            _targetManager.Target = _target1;
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);

            _targetManager.Target = _target1;
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);
        }

        [Test]
        public void SetSecondDifferentTarget_UpdatesHighestPriorityTarget_EmitsEvent()
        {
            _targetManager.Target = _target1;
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);
            Assert.AreEqual(_rankedTarget1, _targetManager.HighestPriorityTarget);

            _targetManager.Target = _target2;
            Assert.AreEqual(2, _highestPriorityTargetChangedCount);
            Assert.AreEqual(_rankedTarget2, _targetManager.HighestPriorityTarget);
        }

        [Test]
        public void TargetDestroyed_UpdatesHighestPriorityTarget_EmitsEvent()
        {
            _targetManager.Target = _target1;
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);
            Assert.AreEqual(_rankedTarget1, _targetManager.HighestPriorityTarget);

            _target1.Destroyed += Raise.EventWith(new DestroyedEventArgs(_target1));
            Assert.AreEqual(2, _highestPriorityTargetChangedCount);
            Assert.IsNull(_targetManager.HighestPriorityTarget);
        }

        [Test]
        public void TargetDestroyed_NotChosenTarget_Throws()
        {
            _targetManager.Target = _target1;
            Assert.Throws<UnityAsserts.AssertionException>(() => _target1.Destroyed += Raise.EventWith(new DestroyedEventArgs(_target2)));
        }

        [Test]
        public void TargetChanged_UnsubsribesFromDestoryedEvent()
        {
            // Set target 1
            _targetManager.Target = _target1;

            // Lose target 1
            _targetManager.Target = null;

            // Target 1 destroyed should no longer be subsribed to
            // First target destroyed event should be unsubsribed
            _highestPriorityTargetChangedCount = 0;
            _target1.Destroyed += Raise.EventWith(new DestroyedEventArgs(_target1));
            Assert.AreEqual(0, _highestPriorityTargetChangedCount);
        }

        [Test]
        public void Dispose_SetsTargetToNull()
        {
            _targetManager.Target = _target1;
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);
            Assert.AreEqual(_rankedTarget1, _targetManager.HighestPriorityTarget);

            _targetManager.DisposeManagedState();
            Assert.AreEqual(2, _highestPriorityTargetChangedCount);
            Assert.IsNull(_targetManager.HighestPriorityTarget);
        }
    }
}