using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public enum IndirectFocusTarget
    {
        None, PlayerCruiser, AICruiser
    }

    /// <summary>
    /// Identical to CameraFocuser, except when focusing on the player or AI
    /// cruisers where the camera will show the overview before going to
    /// the actual cruiser.
    /// </summary>
    public class IndirectCameraFocuser : ICameraFocuser
    {
        private readonly ICameraFocuser _coreFocuser;
        private readonly ICamera _camera;
        private readonly CameraTargetTracker _overviewTargetTracker;
        private IndirectFocusTarget _indirectFocusTarget;

        public const float INDIRECTION_BUFFER_IN_M = 10;

        public IndirectCameraFocuser(ICameraFocuser coreFocuser, ICamera camera, CameraTargetTracker overviewTargetTracker)
        {
            Helper.AssertIsNotNull(coreFocuser, camera, overviewTargetTracker);

            _coreFocuser = coreFocuser;
            _camera = camera;
            _overviewTargetTracker = overviewTargetTracker;
            _indirectFocusTarget = IndirectFocusTarget.None;

            _overviewTargetTracker.IsOnTarget.ValueChanged += IsOnTarget_ValueChanged;
        }

        private void IsOnTarget_ValueChanged(object sender, EventArgs e)
        {
            if (!_overviewTargetTracker.IsOnTarget.Value)
            {
                return;
            }

            switch (_indirectFocusTarget)
            {
                case IndirectFocusTarget.PlayerCruiser:
                    _coreFocuser.FocusOnLeftCruiser();
                    break;

                case IndirectFocusTarget.AICruiser:
                    _coreFocuser.FocusOnRightCruiser();
                    break;
            }

            _indirectFocusTarget = IndirectFocusTarget.None;
        }

        public void FocusOnRightCruiser()
        {
            if (_camera.Position.x + INDIRECTION_BUFFER_IN_M > 0)
            {
                // Direct
                _indirectFocusTarget = IndirectFocusTarget.None;
                _coreFocuser.FocusOnRightCruiser();
            }
            else
            {
                // Indirect
                _indirectFocusTarget = IndirectFocusTarget.AICruiser;
                _coreFocuser.FocusOnOverview();
            }
        }

        public void FocusOnLeftCruiser()
        {
            if (_camera.Position.x - INDIRECTION_BUFFER_IN_M < 0)
            {
                // Direct
                _indirectFocusTarget = IndirectFocusTarget.None;
                _coreFocuser.FocusOnLeftCruiser();
            }
            else
            {
                // Indirect
                _indirectFocusTarget = IndirectFocusTarget.PlayerCruiser;
                _coreFocuser.FocusOnOverview();
            }
        }

        public void FocusMidLeft()
        {
            _indirectFocusTarget = IndirectFocusTarget.None;
            _coreFocuser.FocusMidLeft();
        }

        public void FocusOnRightCruiserDeath()
        {
            _indirectFocusTarget = IndirectFocusTarget.None;
            _coreFocuser.FocusOnRightCruiserDeath();
        }

        public void FocusOnRightCruiserNuke()
        {
            _indirectFocusTarget = IndirectFocusTarget.None;
            _coreFocuser.FocusOnRightCruiserNuke();
        }

        public void FocusOnRightNavalFactory()
        {
            _indirectFocusTarget = IndirectFocusTarget.None;
            _coreFocuser.FocusOnRightNavalFactory();
        }

        public void FocusOnOverview()
        {
            _indirectFocusTarget = IndirectFocusTarget.None;
            _coreFocuser.FocusOnOverview();
        }

        public void FocusOnLeftCruiserDeath()
        {
            _indirectFocusTarget = IndirectFocusTarget.None;
            _coreFocuser.FocusOnLeftCruiserDeath();
        }

        public void FocusOnLeftCruiserNuke()
        {
            _indirectFocusTarget = IndirectFocusTarget.None;
            _coreFocuser.FocusOnLeftCruiserNuke();
        }

        public void FocusOnLeftNavalFactory()
        {
            _indirectFocusTarget = IndirectFocusTarget.None;
            _coreFocuser.FocusOnLeftNavalFactory();
        }
    }
}