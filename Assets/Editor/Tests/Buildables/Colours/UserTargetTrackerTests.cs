using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Colours;
using NSubstitute;
using NUnit.Framework;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Tests.Buildables.Colours
{
    public class UserTargetTrackerTests
    {
        private UserTargetTracker _targetTracker;
        private IBroadcastingProperty<ITarget> _itemShownInInformator;
        private IUserTargets _userTargets;
        private ITarget _target;

        [SetUp]
        public void TestSetup()
        {
            _itemShownInInformator = Substitute.For<IBroadcastingProperty<ITarget>>();
            _userTargets = Substitute.For<IUserTargets>();

            _targetTracker = new UserTargetTracker(_itemShownInInformator, _userTargets);

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
    }
}