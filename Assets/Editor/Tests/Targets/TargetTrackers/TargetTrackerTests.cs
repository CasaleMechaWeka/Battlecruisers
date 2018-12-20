using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetTrackers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetTrackers
{
    public class TargetTrackerTests
    {
        private ITargetTracker _targetTracker;
        private ITargetFinder _targetFinder;
        private ITarget _target;
        private int _targetsChangedCount;

        [SetUp]
        public void TestSetup()
        {
            _targetFinder = Substitute.For<ITargetFinder>();

            _targetTracker = new TargetTracker(_targetFinder);

            _target = Substitute.For<ITarget>();

            _targetsChangedCount = 0;
            _targetTracker.TargetsChanged += (sender, e) => _targetsChangedCount++;
        }

        [Test]
        public void InitialState_NoTargets()
        {
            Assert.IsFalse(_targetTracker.ContainsTarget(_target));
        }

        [Test]
        public void TargetFound_AddsTarget_EmitsChangeEvent()
        {
            _targetFinder.TargetFound += Raise.EventWith(new TargetEventArgs(_target));

            Assert.AreEqual(1, _targetsChangedCount);
            Assert.IsTrue(_targetTracker.ContainsTarget(_target));
        }

        [Test]
        public void TargetFound_Duplicate_DoesNothing()
        {
            // Target 1
            _targetFinder.TargetFound += Raise.EventWith(new TargetEventArgs(_target));
            Assert.AreEqual(1, _targetsChangedCount);
            Assert.IsTrue(_targetTracker.ContainsTarget(_target));

            // Same target again
            _targetFinder.TargetFound += Raise.EventWith(new TargetEventArgs(_target));
            Assert.AreEqual(1, _targetsChangedCount);
        }

        [Test]
        public void TargetLost_RemovesTarget_EmitsChangeEvent()
        {
            // Find target
            _targetFinder.TargetFound += Raise.EventWith(new TargetEventArgs(_target));
            Assert.AreEqual(1, _targetsChangedCount);
            Assert.IsTrue(_targetTracker.ContainsTarget(_target));

            // Lose target
            _targetFinder.TargetLost += Raise.EventWith(new TargetEventArgs(_target));
            Assert.AreEqual(2, _targetsChangedCount);
            Assert.IsFalse(_targetTracker.ContainsTarget(_target));
        }

        [Test]
        public void TargetLost_NotPreviouslyFound_DoesNothing()
        {
            _targetFinder.TargetLost += Raise.EventWith(new TargetEventArgs(_target));
            Assert.AreEqual(0, _targetsChangedCount);
        }

        [Test]
        public void Dispose_Unsubsribes()
        {
            _targetTracker.DisposeManagedState();

            _targetFinder.TargetFound += Raise.EventWith(new TargetEventArgs(_target));
            Assert.AreEqual(0, _targetsChangedCount);
            Assert.IsFalse(_targetTracker.ContainsTarget(_target));
        }
    }
}