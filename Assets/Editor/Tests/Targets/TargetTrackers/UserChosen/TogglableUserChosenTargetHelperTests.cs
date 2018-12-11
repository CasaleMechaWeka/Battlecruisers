using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetTrackers.UserChosen
{
    public class TogglableUserChosenTargetHelperTests
    {
        private IUserChosenTargetHelper _togglabeHelper, _baseHelper;
        private UserChosenTargetHelperPermissions _permissions;
        private ITarget _target;

        [SetUp]
        public void TestSetup()
        {
            _baseHelper = Substitute.For<IUserChosenTargetHelper>();
            _permissions = new UserChosenTargetHelperPermissions(isEnabled: true);

            _togglabeHelper = new TogglableUserChosenTargetHelper(_baseHelper, _permissions);

            _target = Substitute.For<ITarget>();
        }

        [Test]
        public void PermissionDisabled_DoesNotCallBase()
        {
            _permissions.IsEnabled = false;
            _togglabeHelper.ToggleChosenTarget(_target);
            _baseHelper.DidNotReceiveWithAnyArgs().ToggleChosenTarget(null);
        }

        [Test]
        public void PermissionEnabled_CallsBase()
        {
            _permissions.IsEnabled = true;
            _togglabeHelper.ToggleChosenTarget(_target);
            _baseHelper.Received().ToggleChosenTarget(_target);
        }
    }
}