using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
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
    public class PvPIndirectCameraFocuser : IPvPCameraFocuser
    {
        private readonly IPvPCameraFocuser _coreFocuser;
        private readonly IPvPCamera _camera;
        private readonly IPvPCameraTargetTracker _overviewTargetTracker;
        private PvPIndirectFocusTarget _indirectFocusTarget;

        public const float INDIRECTION_BUFFER_IN_M = 10;

        public PvPIndirectCameraFocuser(IPvPCameraFocuser coreFocuser, IPvPCamera camera, IPvPCameraTargetTracker overviewTargetTracker)
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
                    _coreFocuser.FocusOnPlayerCruiser();
                    break;

                case PvPIndirectFocusTarget.EnemyCruiser:
                    _coreFocuser.FocusOnAICruiser();
                    break;
            }

            _indirectFocusTarget = PvPIndirectFocusTarget.None;
        }

        public void FocusOnAICruiser()
        {
            if (_camera.Position.x + INDIRECTION_BUFFER_IN_M > 0)
            {
                // Direct
                _indirectFocusTarget = PvPIndirectFocusTarget.None;
                _coreFocuser.FocusOnAICruiser();
            }
            else
            {
                // Indirect
                _indirectFocusTarget = PvPIndirectFocusTarget.EnemyCruiser;
                _coreFocuser.FocusOnOverview();
            }
        }

        public void FocusOnPlayerCruiser()
        {


            if (_camera.Position.x - INDIRECTION_BUFFER_IN_M < 0)
            {
                // Direct
                _indirectFocusTarget = PvPIndirectFocusTarget.None;
                _coreFocuser.FocusOnPlayerCruiser();
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

        public void FocusOnAICruiserDeath()
        {
            _indirectFocusTarget = PvPIndirectFocusTarget.None;
            _coreFocuser.FocusOnAICruiserDeath();
        }

        public void FocusOnAICruiserNuke()
        {
            _indirectFocusTarget = PvPIndirectFocusTarget.None;
            _coreFocuser.FocusOnAICruiserNuke();
        }

        public void FocusOnAINavalFactory()
        {
            _indirectFocusTarget = PvPIndirectFocusTarget.None;
            _coreFocuser.FocusOnAINavalFactory();
        }

        public void FocusOnOverview()
        {
            _indirectFocusTarget = PvPIndirectFocusTarget.None;
            _coreFocuser.FocusOnOverview();
        }

        public void FocusOnPlayerCruiserDeath()
        {
            _indirectFocusTarget = PvPIndirectFocusTarget.None;
            _coreFocuser.FocusOnPlayerCruiserDeath();
        }

        public void FocusOnPlayerCruiserNuke()
        {
            _indirectFocusTarget = PvPIndirectFocusTarget.None;
            _coreFocuser.FocusOnPlayerCruiserNuke();
        }

        public void FocusOnPlayerNavalFactory()
        {
            _indirectFocusTarget = PvPIndirectFocusTarget.None;
            _coreFocuser.FocusOnPlayerNavalFactory();
        }
    }
}