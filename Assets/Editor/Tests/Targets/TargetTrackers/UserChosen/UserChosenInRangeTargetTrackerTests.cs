using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetTrackers.UserChosen
{
    public class UserChosenInRangeTargetTrackerTests
    {
        private IRankedTargetTracker _userChosenInRangeTargetTracker, _userChosenTargetTracker;
        private ITargetTracker _inRangeTargetTracker;
        private RankedTarget _rankedTarget;

        [SetUp]
        public void TestSetup()
        {
            _userChosenTargetTracker = Substitute.For<IRankedTargetTracker>();
            _inRangeTargetTracker = Substitute.For<ITargetTracker>();

            _userChosenInRangeTargetTracker = new UserChosenInRangeTargetTracker(_inRangeTargetTracker, _userChosenTargetTracker);

            _rankedTarget = new RankedTarget(Substitute.For<ITarget>(), 0);
        }

        [Test]
        public void InitialState()
        {
            Assert.IsNull(_userChosenInRangeTargetTracker.HighestPriorityTarget);
        }

        [Test]
        public void InRangeTargetsChanged_FindsTarget()
        {
            _inRangeTargetTracker.TargetsChanged += Raise.Event();
            RankedTarget compilerBribe = _userChosenTargetTracker.Received().HighestPriorityTarget;
        }

        [Test]
        public void UserChosenTargetChanged_FindsTarget()
        {
            _userChosenTargetTracker.HighestPriorityTargetChanged += Raise.Event();
            RankedTarget compilerBribe = _userChosenTargetTracker.Received().HighestPriorityTarget;
        }

        [Test]
        public void HighestPriorityTarget_NoUserChosenTarget_ReturnsNull()
        {
            _userChosenTargetTracker.HighestPriorityTarget.Returns((RankedTarget)null);

            TriggerReevaluation();

            Assert.IsNull(_userChosenInRangeTargetTracker.HighestPriorityTarget);
        }

        [Test]
        public void HighestPriorityTarget_NoInRangeUserChosenTarget_ReturnsNull()
        {
            _userChosenTargetTracker.HighestPriorityTarget.Returns(_rankedTarget);
            _inRangeTargetTracker.ContainsTarget(_rankedTarget.Target).Returns(false);

            TriggerReevaluation();

            Assert.IsNull(_userChosenInRangeTargetTracker.HighestPriorityTarget);
        }

        [Test]
        public void HighestPriorityTarget_InRangeUserChosenTarget_ReturnsTarget()
        {
            _userChosenTargetTracker.HighestPriorityTarget.Returns(_rankedTarget);
            _inRangeTargetTracker.ContainsTarget(_rankedTarget.Target).Returns(true);

            TriggerReevaluation();

            Assert.AreSame(_userChosenTargetTracker.HighestPriorityTarget, _userChosenInRangeTargetTracker.HighestPriorityTarget);
        }

        private void TriggerReevaluation()
        {
            _inRangeTargetTracker.TargetsChanged += Raise.Event();
        }
    }
}