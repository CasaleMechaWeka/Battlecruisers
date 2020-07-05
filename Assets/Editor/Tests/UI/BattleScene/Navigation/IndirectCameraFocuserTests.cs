using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.BattleScene.Navigation
{
    public class IndirectCameraFocuserTests
    {
        private ICameraFocuser _indirectFocuser, _coreFocuser;
        private ICamera _camera;
        private ICameraTargetTracker _overviewTargetTracker;

        [SetUp]
        public void TestSetup()
        {
            _coreFocuser = Substitute.For<ICameraFocuser>();
            _camera = Substitute.For<ICamera>();
            _overviewTargetTracker = Substitute.For<ICameraTargetTracker>();

            _indirectFocuser
                = new IndirectCameraFocuser(
                    _coreFocuser,
                    _camera,
                    _overviewTargetTracker);
        }

        [Test]
        public void FocusOnAICruiser_WithinBuffer_Direct()
        {
            _camera.Position.Returns(new Vector3(0.1f - IndirectCameraFocuser.INDIRECTION_BUFFER_IN_M, 0, 0));
            _indirectFocuser.FocusOnAICruiser();
            _coreFocuser.Received().FocusOnAICruiser();
        }

        [Test]
        public void FocusOnAICruiser_BeyondBuffer_Indirect()
        {
            _camera.Position.Returns(new Vector3(-IndirectCameraFocuser.INDIRECTION_BUFFER_IN_M, 0, 0));
            _indirectFocuser.FocusOnAICruiser();
            _coreFocuser.Received().FocusOnOverview();
        }

        [Test]
        public void FocusOnPlayerCruiser_WithinBuffer_Direct()
        {
            _camera.Position.Returns(new Vector3(IndirectCameraFocuser.INDIRECTION_BUFFER_IN_M - 0.1f, 0, 0));
            _indirectFocuser.FocusOnPlayerCruiser();
            _coreFocuser.Received().FocusOnPlayerCruiser();
        }

        [Test]
        public void FocusOnPlayerCruiser_BeyondBuffer_Indirect()
        {
            _camera.Position.Returns(new Vector3(IndirectCameraFocuser.INDIRECTION_BUFFER_IN_M, 0, 0));
            _indirectFocuser.FocusOnPlayerCruiser();
            _coreFocuser.Received().FocusOnOverview();
        }

        #region IsOnTarget_ValueChanged
        [Test]
        public void IsOnTarget_ValueChanged_NotOnTarget()
        {
            _overviewTargetTracker.IsOnTarget.Value.Returns(false);
            
            _overviewTargetTracker.IsOnTarget.ValueChanged += Raise.Event();
            
            _coreFocuser.DidNotReceive().FocusOnPlayerCruiser();
            _coreFocuser.DidNotReceive().FocusOnAICruiser();
        }

        [Test]
        public void IsOnTarget_ValueChanged_OnTarget_NoIndirectFocusTarget()
        {
            _overviewTargetTracker.IsOnTarget.Value.Returns(true);

            _overviewTargetTracker.IsOnTarget.ValueChanged += Raise.Event();

            _coreFocuser.DidNotReceive().FocusOnPlayerCruiser();
            _coreFocuser.DidNotReceive().FocusOnAICruiser();
        }

        [Test]
        public void IsOnTarget_ValueChanged_OnTarget_PlayerCruiserIndirectFocusTarget()
        {
            MakePlayerCruiserIndirectTarget();
            _overviewTargetTracker.IsOnTarget.Value.Returns(true);

            _overviewTargetTracker.IsOnTarget.ValueChanged += Raise.Event();

            _coreFocuser.Received().FocusOnPlayerCruiser();

            // Check indirect target is cleared
            _coreFocuser.ClearReceivedCalls();
            _overviewTargetTracker.IsOnTarget.ValueChanged += Raise.Event();
            _coreFocuser.DidNotReceive().FocusOnPlayerCruiser();
        }

        private void MakePlayerCruiserIndirectTarget()
        {
            FocusOnPlayerCruiser_BeyondBuffer_Indirect();
        }

        [Test]
        public void IsOnTarget_ValueChanged_OnTarget_AICruiserIndirectFocusTarget()
        {
            MakeAICruiserIndirectTarget();
            _overviewTargetTracker.IsOnTarget.Value.Returns(true);

            _overviewTargetTracker.IsOnTarget.ValueChanged += Raise.Event();

            _coreFocuser.Received().FocusOnAICruiser();

            // Check indirect target is cleared
            _coreFocuser.ClearReceivedCalls();
            _overviewTargetTracker.IsOnTarget.ValueChanged += Raise.Event();
            _coreFocuser.DidNotReceive().FocusOnAICruiser();
        }

        private void MakeAICruiserIndirectTarget()
        {
            FocusOnAICruiser_BeyondBuffer_Indirect();
        }
        #endregion IsOnTarget_ValueChanged
    }
}