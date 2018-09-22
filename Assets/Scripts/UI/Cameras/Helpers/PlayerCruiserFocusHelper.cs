using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Helpers
{
    // FELIX  Test :)
    public class PlayerCruiserFocusHelper : IPlayerCruiserFocusHelper
    {
        private readonly ICamera _camera;
        private readonly ICameraController _cameraController;
        private readonly ICruiser _playerCruiser;

        private const float CAMERA_MARGIN_IN_M = 10;

        public PlayerCruiserFocusHelper(ICamera camera, ICameraController cameraController, ICruiser playerCruiser)
        {
            Helper.AssertIsNotNull(camera, cameraController, playerCruiser);

            _camera = camera;
            _cameraController = cameraController;
            _playerCruiser = playerCruiser;
        }

        public void FocusOnPlayerCruiserIfNeeded()
        {
            if (!IsCameraRoughlyOnPlayerCruiser())
            {
                _cameraController.FocusOnPlayerCruiser();
            }
        }

        private bool IsCameraRoughlyOnPlayerCruiser()
        {
            return Vector2.Distance(_camera.Position, _playerCruiser.Position) < CAMERA_MARGIN_IN_M;
        }
    }
}