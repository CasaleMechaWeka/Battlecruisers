using BattleCruisers.Buildables;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProviders;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Targets.TargetFinders
{
    public class UserChosenTargetManagerTests
    {
        private IUserChosenTargetManager _targetManager;
        private ITargetConsumer _targetConsumer;
        private ITargetProvider _targetProvider;
        private ITarget _target, _target2, _expectedTargetFound, _expectedTargetLost;
        private int _targetFoundCount, _targetLostCount;

        [SetUp]
        public void TestSetup()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _targetManager = new UserChosenTargetManager();
            _targetConsumer = _targetManager;
            _targetProvider = _targetManager;

            _target = Substitute.For<ITarget>();
            _target2 = Substitute.For<ITarget>();

            _targetFoundCount = 0;
            _targetLostCount = 0;

            _targetManager.TargetFound += _targetManager_TargetFound;
            _targetManager.TargetLost += _targetManager_TargetLost;
        }

        private void _targetManager_TargetFound(object sender, TargetEventArgs e)
        {
            Assert.AreSame(_expectedTargetFound, e.Target);
            _targetFoundCount++;
        }

        private void _targetManager_TargetLost(object sender, TargetEventArgs e)
        {
            Assert.AreSame(_expectedTargetLost, e.Target);
            _targetLostCount++;
        }

        [Test]
        public void InitialState()
        {
            Assert.IsNull(_targetProvider.Target);
        }

        [Test]
        public void SetFirstTarget()
        {
            SetTarget(_target);

            Assert.AreEqual(1, _targetFoundCount);
            Assert.AreSame(_target, _targetProvider.Target);
        }

        [Test]
        public void SetTargetToNull()
        {
            SetTarget(_target);

            _expectedTargetLost = _target;

            _targetConsumer.Target = null;

            Assert.AreEqual(1, _targetLostCount);
            Assert.IsNull(_targetProvider.Target);
        }

        [Test]
        public void SetSecondSameTarget()
        {
            SetTarget(_target);

            _targetFoundCount = 0;
            _targetConsumer.Target = _target;
            Assert.AreEqual(0, _targetFoundCount);
        }

        [Test]
        public void SetSecondDifferentTarget()
        {
            SetTarget(_target);

            _targetFoundCount = 0;
            _expectedTargetLost = _target;
            _expectedTargetFound = _target2;

            _targetConsumer.Target = _target2;

            Assert.AreEqual(1, _targetLostCount);
            Assert.AreEqual(1, _targetFoundCount);
            Assert.AreSame(_target2, _targetProvider.Target);
        }

        [Test]
        public void TargetDestroyed()
        {
            SetTarget(_target);

            _expectedTargetLost = _target;

            _target.Destroyed += Raise.EventWith(new DestroyedEventArgs(_target));

            Assert.AreEqual(1, _targetLostCount);
            Assert.IsNull(_targetProvider.Target);
        }

        [Test]
        public void TargetDestroyed_NotChosenTarget_Throws()
        {
            SetTarget(_target);

            Assert.Throws<UnityAsserts.AssertionException>(() => _target.Destroyed += Raise.EventWith(new DestroyedEventArgs(_target2)));
        }

        [Test]
        public void TargetLost_UnsubsribesFromDestoryedEvent()
        {
            // First target destroyed event should be unsubsribed
            SetSecondDifferentTarget();

            _targetLostCount = 0;
            _target.Destroyed += Raise.EventWith(new DestroyedEventArgs(_target));
            Assert.AreEqual(0, _targetLostCount);
        }

        [Test]
        public void Dispose_SetsTargetToNull()
        {
            SetTarget(_target);

            _expectedTargetLost = _target;

            _targetManager.DisposeManagedState();

            Assert.AreEqual(1, _targetLostCount);
            Assert.IsNull(_targetProvider.Target);
        }

        private void SetTarget(ITarget target)
        {
            _expectedTargetFound = target;
            _targetConsumer.Target = target;
        }
    }
}