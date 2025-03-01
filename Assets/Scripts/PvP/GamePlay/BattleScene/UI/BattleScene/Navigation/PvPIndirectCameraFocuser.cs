using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.Utils.PlatformAbstractions;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public enum PvPIndirectFocusTarget
    {
        None, PlayerCruiser, EnemyCruiser
    }

    /// <summary>
    /// Identical to CameraFocuser, except when focusing on the player or AI
    /// cruisers where the camera will show the overview before going to
    /// the actual cruiser.
    /// </summary>
    public class PvPIndirectCameraFocuser : ICameraFocuser
    {
        private readonly ICameraFocuser _coreFocuser;
        private readonly ICamera _camera;
        private readonly ICameraTargetTracker _overviewTargetTracker;
        private PvPIndirectFocusTarget _indirectFocusTarget;

        public const float INDIRECTION_BUFFER_IN_M = 10;

        public PvPIndirectCameraFocuser(ICameraFocuser coreFocuser, ICamera camera, ICameraTargetTracker overviewTargetTracker)
        {
            PvPHelper.AssertIsNotNull(coreFocuser, camera, overviewTargetTracker);

            _coreFocuser = coreFocuser;
            _camera = camera;
            _overviewTargetTracker = overviewTargetTracker;
            _indirectFocusTarget = PvPIndirectFocusTarget.None;

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
                case PvPIndirectFocusTarget.PlayerCruiser:
                    _coreFocuser.FocusOnLeftCruiser();
                    break;

                case PvPIndirectFocusTarget.EnemyCruiser:
                    _coreFocuser.FocusOnRightCruiser();
                    break;
            }

            _indirectFocusTarget = PvPIndirectFocusTarget.None;
        }

        public void FocusOnRightCruiser()
        {
            if (_camera.Position.x + INDIRECTION_BUFFER_IN_M > 0)
            {
                // Direct
                _indirectFocusTarget = PvPIndirectFocusTarget.None;
                _coreFocuser.FocusOnRightCruiser();
            }
            else
            {
                // Indirect
                _indirectFocusTarget = PvPIndirectFocusTarget.EnemyCruiser;
                _coreFocuser.FocusOnOverview();
            }
        }

        public void FocusOnLeftCruiser()
        {


            if (_camera.Position.x - INDIRECTION_BUFFER_IN_M < 0)
            {
                // Direct
                _indirectFocusTarget = PvPIndirectFocusTarget.None;
                _coreFocuser.FocusOnLeftCruiser();
            }
            else
            {
                // Indirect
                _indirectFocusTarget = PvPIndirectFocusTarget.PlayerCruiser;
                _coreFocuser.FocusOnOverview();
            }
        }

        public void FocusMidLeft()
        {
            _indirectFocusTarget = PvPIndirectFocusTarget.None;
            _coreFocuser.FocusMidLeft();
        }

        public void FocusOnRightCruiserDeath()
        {
            _indirectFocusTarget = PvPIndirectFocusTarget.None;
            _coreFocuser.FocusOnRightCruiserDeath();
        }

        public void FocusOnRightCruiserNuke()
        {
            _indirectFocusTarget = PvPIndirectFocusTarget.None;
            _coreFocuser.FocusOnRightCruiserNuke();
        }

        public void FocusOnRightNavalFactory()
        {
            _indirectFocusTarget = PvPIndirectFocusTarget.None;
            _coreFocuser.FocusOnRightNavalFactory();
        }

        public void FocusOnOverview()
        {
            _indirectFocusTarget = PvPIndirectFocusTarget.None;
            _coreFocuser.FocusOnOverview();
        }

        public void FocusOnLeftCruiserDeath()
        {
            _indirectFocusTarget = PvPIndirectFocusTarget.None;
            _coreFocuser.FocusOnLeftCruiserDeath();
        }

        public void FocusOnLeftCruiserNuke()
        {
            _indirectFocusTarget = PvPIndirectFocusTarget.None;
            _coreFocuser.FocusOnLeftCruiserNuke();
        }

        public void FocusOnLeftNavalFactory()
        {
            _indirectFocusTarget = PvPIndirectFocusTarget.None;
            _coreFocuser.FocusOnLeftNavalFactory();
        }
    }
}