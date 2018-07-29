using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetFinders
{
    public class CompositeTargetFinderTests
    {
        private ITargetFinder _compositeFinder, _finder1, _finder2;
        private ITarget _target, _expectedTargetFound, _expectedTargetLost;
        private int _targetFoundCount, _targetLostCount;

        [SetUp]
        public void TestSetup()
        {
            _finder1 = Substitute.For<ITargetFinder>();
            _finder2 = Substitute.For<ITargetFinder>();
            _compositeFinder = new CompositeTargetFinder(_finder1, _finder2);

            _targetFoundCount = 0;
            _targetLostCount = 0;

            _compositeFinder.TargetFound += _compositeFinder_TargetFound;
            _compositeFinder.TargetLost += _compositeFinder_TargetLost;

            _target = Substitute.For<ITarget>();
        }

        private void _compositeFinder_TargetFound(object sender, TargetEventArgs e)
        {
            Assert.AreSame(_expectedTargetFound, e.Target);
            _targetFoundCount++;
        }

        private void _compositeFinder_TargetLost(object sender, TargetEventArgs e)
        {
            Assert.AreSame(_expectedTargetLost, e.Target);
            _targetLostCount++;
        }

        [Test]
        public void StartFindingTargets_Propagates()
        {
            _compositeFinder.StartFindingTargets();

            _finder1.Received().StartFindingTargets();
            _finder2.Received().StartFindingTargets();
        }

        [Test]
        public void Dispose_Propagates_AndUnsubsribes()
        {
            _compositeFinder.DisposeManagedState();

            _finder1.Received().DisposeManagedState();
            _finder2.Received().DisposeManagedState();

            _finder1.TargetFound += Raise.EventWith(new TargetEventArgs(_target));
            _finder1.TargetLost += Raise.EventWith(new TargetEventArgs(_target));
            _finder2.TargetFound += Raise.EventWith(new TargetEventArgs(_target));
            _finder2.TargetLost += Raise.EventWith(new TargetEventArgs(_target));

            Assert.AreEqual(0, _targetFoundCount);
            Assert.AreEqual(0, _targetLostCount);
        }

        [Test]
        public void TargetFound_Propogates()
        {
            _expectedTargetFound = _target;

            _finder1.TargetFound += Raise.EventWith(new TargetEventArgs(_target));
            _finder2.TargetFound += Raise.EventWith(new TargetEventArgs(_target));

            Assert.AreEqual(2, _targetFoundCount);
        }

        [Test]
        public void TargetLost_Propogates()
        {
            _expectedTargetLost = _target;

            _finder1.TargetLost += Raise.EventWith(new TargetEventArgs(_target));
            _finder2.TargetLost += Raise.EventWith(new TargetEventArgs(_target));

            Assert.AreEqual(2, _targetLostCount);
        }
    }
}