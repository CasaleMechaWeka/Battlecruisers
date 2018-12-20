using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetFinders
{
    public class MinRangeTargetFinderTests
    {
        private ITargetFinder _targetFinder;
        private ITargetDetector _maxRangeDetector, _minRangeDetector;
        private ITarget _target;
        private ITargetFilter _targetFilter;

        [SetUp]
        public void TestSetup()
        {
            _maxRangeDetector = Substitute.For<ITargetDetector>();
            _minRangeDetector = Substitute.For<ITargetDetector>();
            _target = Substitute.For<ITarget>();

            _targetFilter = Substitute.For<ITargetFilter>();

            _targetFinder = new MinRangeTargetFinder(_maxRangeDetector, _minRangeDetector, _targetFilter);
        }

        #region Max range detector
        [Test]
        public void MaxRange_TargetEntered_IsMatch_EmitsTargetFound()
        {
            bool wasCalled = false;

            _targetFinder.TargetFound += (sender, e) =>
            {
                wasCalled = true;
                Assert.AreEqual(_targetFinder, sender);
                Assert.AreEqual(_target, e.Target);
            };

            _targetFilter.IsMatch(_target).Returns(true);
            _maxRangeDetector.OnEntered += Raise.EventWith(_maxRangeDetector, new TargetEventArgs(_target));

            Assert.IsTrue(wasCalled);
        }

        [Test]
        public void MaxRange_TargetEntered_IsNotMatch_EmitsNothing()
        {
            _targetFinder.TargetFound += (sender, e) =>
            {
                Assert.Fail();
            };

            _targetFilter.IsMatch(_target).Returns(false);
            _maxRangeDetector.OnEntered += Raise.EventWith(_maxRangeDetector, new TargetEventArgs(_target));
        }

        [Test]
        public void MaxRange_TargetExited_IsMatch_EmitsTargetLost()
        {
            bool wasCalled = false;

            _targetFinder.TargetLost += (sender, e) =>
            {
                wasCalled = true;
                Assert.AreEqual(_targetFinder, sender);
                Assert.AreEqual(_target, e.Target);
            };

            _targetFilter.IsMatch(_target).Returns(true);
            _maxRangeDetector.OnExited += Raise.EventWith(_maxRangeDetector, new TargetEventArgs(_target));

            Assert.IsTrue(wasCalled);
        }

        [Test]
        public void MaxRange_TargetExited_IsNotMatch_EmitsNothing()
        {
            _targetFinder.TargetLost += (sender, e) =>
            {
                Assert.Fail();
            };

            _targetFilter.IsMatch(_target).Returns(false);
            _maxRangeDetector.OnExited += Raise.EventWith(_maxRangeDetector, new TargetEventArgs(_target));
        }
        #endregion Max range detector

        #region Min range detector
        [Test]
        public void MinRange_TargetEntered_IsMatch_EmitsTargetLost()
        {
            bool wasCalled = false;

            _targetFinder.TargetLost += (sender, e) =>
            {
                wasCalled = true;
                Assert.AreEqual(_targetFinder, sender);
                Assert.AreEqual(_target, e.Target);
            };

            _targetFilter.IsMatch(_target).Returns(true);
            _minRangeDetector.OnEntered += Raise.EventWith(_minRangeDetector, new TargetEventArgs(_target));

            Assert.IsTrue(wasCalled);
        }

        [Test]
        public void MinRange_TargetEntered_IsNotMatch_DoesNothing()
        {
            _targetFinder.TargetLost += (sender, e) =>
            {
                Assert.Fail();
            };

            _targetFilter.IsMatch(_target).Returns(false);
            _minRangeDetector.OnEntered += Raise.EventWith(_minRangeDetector, new TargetEventArgs(_target));
        }

        [Test]
        public void MinRange_TargetExited_Destroyed_DoesNothing()
        {
            _targetFinder.TargetFound += (sender, e) => 
            {
                Assert.Fail();
            };

            _targetFilter.IsMatch(_target).Returns(true);
            _target.IsDestroyed.Returns(true);
            _minRangeDetector.OnExited += Raise.EventWith(_minRangeDetector, new TargetEventArgs(_target));
        }

        [Test]
        public void MinRange_TargetExited_IsNotMatch_DoesNothing()
        {
            _targetFinder.TargetFound += (sender, e) =>
            {
                Assert.Fail();
            };

            _targetFilter.IsMatch(_target).Returns(false);
            _target.IsDestroyed.Returns(false);
            _minRangeDetector.OnExited += Raise.EventWith(_minRangeDetector, new TargetEventArgs(_target));
        }

        [Test]
        public void MinRange_TargetExited_NotDestroyed_IsMatch_EmitsTargetFound()
        {
            bool wasCalled = false;

            _targetFinder.TargetFound += (sender, e) =>
            {
                wasCalled = true;
                Assert.AreEqual(_targetFinder, sender);
                Assert.AreEqual(_target, e.Target);
            };

            _targetFilter.IsMatch(_target).Returns(true);
            _target.IsDestroyed.Returns(false);
            _minRangeDetector.OnExited += Raise.EventWith(_minRangeDetector, new TargetEventArgs(_target));

            Assert.IsTrue(wasCalled);
        }
        #endregion Min range detector
    }
}
