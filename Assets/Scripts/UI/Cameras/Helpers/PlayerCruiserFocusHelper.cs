using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class PlayerCruiserFocusHelper : IPlayerCruiserFocusHelper
    {
        private readonly ICamera _camera;
        private readonly ICameraFocuser _cameraFocuser;
        private readonly ICruiser _playerCruiser;

        private const float CAMERA_MARGIN_IN_M = 10;

        public PlayerCruiserFocusHelper(ICamera camera, ICameraFocuser cameraFocuser, ICruiser playerCruiser)
        {
            Helper.AssertIsNotNull(camera, cameraFocuser, playerCruiser);

            _camera = camera;
            _cameraFocuser = cameraFocuser;
            _playerCruiser = playerCruiser;
        }

        public void FocusOnPlayerCruiserIfNeeded()
        {
            if (!IsCameraRoughlyOnPlayerCruiser())
            {
                _cameraFocuser.FocusOnPlayerCruiser();
            }
        }

        private bool IsCameraRoughlyOnPlayerCruiser()
        {
            return Vector2.Distance(_camera.Transform.Position, _playerCruiser.Position) < CAMERA_MARGIN_IN_M;
        }
    }
}