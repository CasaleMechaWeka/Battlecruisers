using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetTrackers.UserChosen
{
    public class TogglableUserChosenTargetHelperTests
    {
        private IUserChosenTargetHelper _togglabeHelper, _baseHelper;
        private ITarget _target;

        [SetUp]
        public void TestSetup()
        {
            _baseHelper = Substitute.For<IUserChosenTargetHelper>();

            _togglabeHelper = new TogglableUserChosenTargetHelper(_baseHelper, true);

            _target = Substitute.For<ITarget>();
        }

        [Test]
        public void PermissionDisabled_DoesNotCallBase()
        {
            _togglabeHelper.ToggleChosenTarget(_target);
            _baseHelper.DidNotReceiveWithAnyArgs().ToggleChosenTarget(null);
        }

        [Test]
        public void PermissionEnabled_CallsBase()
        {
            _togglabeHelper.ToggleChosenTarget(_target);
            _baseHelper.Received().ToggleChosenTarget(_target);
        }

        [Test]
        public void UserChosenTarget_ForwardsBase()
        {
            ITarget userChosenTarget = Substitute.For<ITarget>();
            _baseHelper.UserChosenTarget.Returns(userChosenTarget);

            Assert.AreSame(userChosenTarget, _togglabeHelper.UserChosenTarget);
        }

        [Test]
        public void BaseUserChosenTargetChanged_GetsForwarded()
        {
            int eventCount = 0;
            _togglabeHelper.UserChosenTargetChanged += (sender, e) => eventCount++;

            _baseHelper.UserChosenTargetChanged += Raise.Event();

            Assert.AreEqual(1, eventCount);
        }
    }
}