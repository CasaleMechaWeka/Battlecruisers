using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Colours;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using UnityCommon.Properties;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.Colours
{
    public class UserTargetTrackerTests
    {
        private UserTargetTracker _targetTracker;
        private IBroadcastingProperty<ITarget> _itemShownInInformator;
        private IRankedTargetTracker _userChosenTargetTracker;
        private IUserTargets _userTargets;
        private ITarget _target;

        [SetUp]
        public void TestSetup()
        {
            _itemShownInInformator = Substitute.For<IBroadcastingProperty<ITarget>>();
            _userChosenTargetTracker = Substitute.For<IRankedTargetTracker>();
            _userTargets = Substitute.For<IUserTargets>();

            _targetTracker = new UserTargetTracker(_itemShownInInformator, _userChosenTargetTracker, _userTargets);

            _target = Substitute.For<ITarget>();
        }

        #region InformatorItemChanged
        [Test]
        public void InformatorItemChanged_ClearsSelectedTarget()
        {
            _itemShownInInformator.ValueChanged += Raise.Event();
            _userTargets.Received().SelectedTarget = null;
        }

        [Test]
        public void InformatorItemChanged_NewValue_IsNull()
        {
            _itemShownInInformator.Value.Returns((ITarget)null);

            _itemShownInInformator.ValueChanged += Raise.Event();

            _userTargets.DidNotReceive().SelectedTarget = _target;
        }

        [Test]
        public void InformatorItemChanged_NewValue_IsNotNull_IsNotInScene()
        {
            _target.IsInScene.Returns(false);
            _itemShownInInformator.Value.Returns(_target);

            _itemShownInInformator.ValueChanged += Raise.Event();

            _userTargets.DidNotReceive().SelectedTarget = _target;
        }

        [Test]
        public void InformatorItemChanged_NewValue_IsNotNull_IsInScene()
        {
            _target.IsInScene.Returns(true);
            _itemShownInInformator.Value.Returns(_target);

            _itemShownInInformator.ValueChanged += Raise.Event();

            _userTargets.Received().SelectedTarget = _target;
        }
        #endregion InformatorItemChanged

        #region UserChosenTargetChanged
        [Test]
        public void UserChosenTargetChanged_ClearsTargetToAttack()
        {
            _userChosenTargetTracker.HighestPriorityTargetChanged += Raise.Event();
            _userTargets.Received().TargetToAttack = null;
        }

        [Test]
        public void UserChosenTargetChanged_NewValue_IsNull()
        {
            _userChosenTargetTracker.HighestPriorityTarget.Returns((RankedTarget)null);

            _userChosenTargetTracker.HighestPriorityTargetChanged += Raise.Event();

            _userTargets.DidNotReceive().TargetToAttack = _target;
        }

        [Test]
        public void UserChosenTargetChanged_NewValue_IsNotNull()
        {
            RankedTarget rankedTarget = new RankedTarget(_target, 1);
            _userChosenTargetTracker.HighestPriorityTarget.Returns(rankedTarget);

            _userChosenTargetTracker.HighestPriorityTargetChanged += Raise.Event();

            _userTargets.Received().TargetToAttack = _target;
        }
        #endregion UserChosenTargetChanged
    }
}