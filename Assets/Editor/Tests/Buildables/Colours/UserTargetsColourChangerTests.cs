using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Colours;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

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
        public void SelectedTarget_OldValue_Null()
        {
            _userTargets.SelectedTarget = null;
        }

        [Test]
        public void SelectedTarget_OldValue_NotNull_SameAsTargetToAttack()
        {
            _userTargets.SelectedTarget = _target1;
            _userTargets.TargetToAttack = _target1;

            _target1.ClearReceivedCalls();

            _userTargets.SelectedTarget = null;

            _target1.DidNotReceiveWithAnyArgs().Color = default;
        }

        [Test]
        public void SelectedTarget_OldValue_NotNull_DifferentToTargetToAttack()
        {
            _userTargets.SelectedTarget = _target1;
            _userTargets.TargetToAttack = _target2;

            _target1.ClearReceivedCalls();

            _userTargets.SelectedTarget = null;

            _target1.Received().Color = TargetColours.Default;
        }

        [Test]
        public void SelectedTarget_NewValue_Null()
        {
            _userTargets.SelectedTarget = null;
        }

        [Test]
        public void SelectedTarget_NewValue_NotNull_SameAsTargetToAttack()
        {
            _userTargets.TargetToAttack = _target1;
            _target1.ClearReceivedCalls();

            _userTargets.SelectedTarget = _target1;

            _target1.DidNotReceiveWithAnyArgs().Color = default;
        }

        [Test]
        public void SelectedTarget_NewValue_NotNull_DifferentToTargetToAttack()
        {
            _userTargets.TargetToAttack = _target1;
            _userTargets.SelectedTarget = _target2;

            _target2.Received().Color = TargetColours.Selected;
        }
        #endregion SelectedTarget

        #region TargetToAttack
        [Test]
        public void TargetToAttack_OldValue_Null()
        {
            _userTargets.TargetToAttack = null;
        }

        [Test]
        public void TargetToAttack_OldValue_NotNull_IsCurrentlySelected()
        {
            _userTargets.TargetToAttack = _target1;
            _userTargets.SelectedTarget = _target1;

            _target1.ClearReceivedCalls();

            _userTargets.TargetToAttack = null;
            _target1.Received().Color = TargetColours.Selected;
        }

        [Test]
        public void TargetToAttack_OldValue_NotNull_IsNotCurrentlySelected()
        {
            _userTargets.TargetToAttack = _target1;
            _userTargets.SelectedTarget = _target2;

            _target1.ClearReceivedCalls();

            _userTargets.TargetToAttack = null;
            _target1.Received().Color = TargetColours.Default;
        }

        [Test]
        public void TargetToAttack_NewValue_Null()
        {
            _userTargets.TargetToAttack = null;
        }

        [Test]
        public void TargetToAttack_NewValue_NotNull()
        {
            _userTargets.TargetToAttack = _target1;
            _target1.Received().Color = TargetColours.Targetted;
        }
        #endregion TargetToAttack
    }
}