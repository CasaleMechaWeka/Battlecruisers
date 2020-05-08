using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Colours;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.Colours
{
    public class UserTargetsColourChangerTests
    {
        private IUserTargets _userTargets;
        private ITarget _target1, _target2;

        [SetUp]
        public void TestSetup()
        {
            _userTargets = new UserTargetsColourChanger();

            _target1 = Substitute.For<ITarget>();
            _target2 = Substitute.For<ITarget>();
        }

        #region SelectedTarget
        [Test]
        public void SelectedTarget_Null_To_Null()
        {
            _userTargets.SelectedTarget = null;
        }

        [Test]
        public void SelectedTarget_Null_To_NotNull()
        {
            _userTargets.SelectedTarget = _target2;
            _target2.Received().Color = TargetColours.Selected;
        }

        [Test]
        public void SelectedTarget_NotNull_To_Null()
        {
            _userTargets.SelectedTarget = _target1;
            _target1.ClearReceivedCalls();

            _userTargets.SelectedTarget = null;

            _target1.Received().Color = TargetColours.Default;
        }
        #endregion SelectedTarget
    }
}