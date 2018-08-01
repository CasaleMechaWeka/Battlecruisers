using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Targets.TargetTrackers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetTrackers
{
    public class CompositeTrackerTests
    {
        private IHighestPriorityTargetTracker _compositeTracker, _tracker1, _tracker2;
        private RankedTarget _target1, _target2;
        private int _highestPriorityTargetChangedCount;

        [SetUp]
        public void TestSetup()
        {
            _tracker1 = Substitute.For<IHighestPriorityTargetTracker>();
            _tracker2 = Substitute.For<IHighestPriorityTargetTracker>();
            _compositeTracker = new CompositeTracker(_tracker1, _tracker2);

            _target1 = new RankedTarget(null, 0);
            _target2 = new RankedTarget(null, 1);

            _highestPriorityTargetChangedCount = 0;
            _compositeTracker.HighestPriorityTargetChanged += (sender, e) => _highestPriorityTargetChangedCount++;
        }

        [Test]
        public void InitialState()
        {
            Assert.IsNull(_compositeTracker.HighestPriorityTarget);
        }

        [Test]
        public void HighestPriorityTargetChanged_UpdatesTarget_AndEmitsEvent()
        {
            _tracker1.HighestPriorityTarget.Returns(_target1);
            _tracker1.HighestPriorityTargetChanged += Raise.Event();

            Assert.AreSame(_target1, _compositeTracker.HighestPriorityTarget);
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);
        }

        [Test]
        public void HighestPriorityTarget_ChoosesHighestOfTrackers()
        {
            _tracker1.HighestPriorityTarget.Returns(_target1);
            _tracker1.HighestPriorityTargetChanged += Raise.Event();

            _tracker2.HighestPriorityTarget.Returns(_target2);
            _tracker2.HighestPriorityTargetChanged += Raise.Event();

            Assert.AreSame(_target2, _compositeTracker.HighestPriorityTarget);
            Assert.AreEqual(2, _highestPriorityTargetChangedCount);
        }

        [Test]
        public void LowerPriorityTarget_DoesNotEmitEvent()
        {
            _tracker2.HighestPriorityTarget.Returns(_target2);
            _tracker2.HighestPriorityTargetChanged += Raise.Event();
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);
            Assert.AreSame(_target2, _compositeTracker.HighestPriorityTarget);

            _tracker1.HighestPriorityTarget.Returns(_target1);
            _tracker1.HighestPriorityTargetChanged += Raise.Event();
            Assert.AreEqual(1, _highestPriorityTargetChangedCount);
            Assert.AreSame(_target2, _compositeTracker.HighestPriorityTarget);
        }

        [Test]
        public void StartTrackingTargets_Propagates()
        {
            _compositeTracker.StartTrackingTargets();

            _tracker1.Received().StartTrackingTargets();
            _tracker2.Received().StartTrackingTargets();
        }

        [Test]
        public void Dispose_Propagates()
        {
            _compositeTracker.DisposeManagedState();

            _tracker1.Received().DisposeManagedState();
            _tracker2.Received().DisposeManagedState();
        }
    }
}