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
    /// FELIX  Use, test
    public class IndirectCameraFocuser : ICameraFocuser
    {
        private readonly ICameraFocuser _coreFocuser;
        private readonly ICamera _camera;
        private readonly ICameraTargetTracker _overviewTargetTracker;
        private IndirectFocusTarget _indirectFocusTarget;

        public const float INDIRECTION_BUFFER_IN_M = 10;

        public IndirectCameraFocuser(ICameraFocuser coreFocuser, ICamera camera, ICameraTargetTracker overviewTargetTracker)
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
                    _coreFocuser.FocusOnPlayerCruiser();
                    break;

                case IndirectFocusTarget.AICruiser:
                    _coreFocuser.FocusOnAICruiser();
                    break;
            }

            _indirectFocusTarget = IndirectFocusTarget.None;
        }

        public void FocusOnAICruiser()
        {
            if (_camera.Position.x + INDIRECTION_BUFFER_IN_M > 0)
            {
                // Direct
                _indirectFocusTarget = IndirectFocusTarget.None;
                _coreFocuser.FocusOnAICruiser();
            }
            else
            {
                // Indirect
                _indirectFocusTarget = IndirectFocusTarget.AICruiser;
                _coreFocuser.FocusOnOverview();
            }
        }

        public void FocusOnPlayerCruiser()
        {
            if (_camera.Position.x - INDIRECTION_BUFFER_IN_M < 0)
            {
                // Direct
                _indirectFocusTarget = IndirectFocusTarget.None;
                _coreFocuser.FocusOnPlayerCruiser();
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

        public void FocusOnAICruiserDeath()
        {
            _indirectFocusTarget = IndirectFocusTarget.None;
            _coreFocuser.FocusOnAICruiserDeath();
        }

        public void FocusOnAICruiserNuke()
        {
            _indirectFocusTarget = IndirectFocusTarget.None;
            _coreFocuser.FocusOnAICruiserNuke();
        }

        public void FocusOnAINavalFactory()
        {
            _indirectFocusTarget = IndirectFocusTarget.None;
            _coreFocuser.FocusOnAINavalFactory();
        }

        public void FocusOnOverview()
        {
            _indirectFocusTarget = IndirectFocusTarget.None;
            _coreFocuser.FocusOnOverview();
        }

        public void FocusOnPlayerCruiserDeath()
        {
            _indirectFocusTarget = IndirectFocusTarget.None;
            _coreFocuser.FocusOnPlayerCruiserDeath();
        }

        public void FocusOnPlayerCruiserNuke()
        {
            _indirectFocusTarget = IndirectFocusTarget.None;
            _coreFocuser.FocusOnPlayerCruiserNuke();
        }

        public void FocusOnPlayerNavalFactory()
        {
            _indirectFocusTarget = IndirectFocusTarget.None;
            _coreFocuser.FocusOnPlayerNavalFactory();
        }
    }
}