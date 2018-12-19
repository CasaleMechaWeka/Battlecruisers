using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers
{
    // FELIX  Update tests :)
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

        public void FocusOnPlayerNavalFactoryIfNeeded()
        {
            if (!IsCameraRoughlyOnPlayerNavalFactory())
            {
                _cameraFocuser.FocusOnPlayerNavalFactory();
            }
        }

        private bool IsCameraRoughlyOnPlayerNavalFactory()
        {
            ISlot bowSlot 
                = _playerCruiser.SlotAccessor
                    .GetSlots(SlotType.Bow)
                    .FirstOrDefault();

            Assert.IsNotNull(bowSlot);

            // FELI  If don't end up adjusting margin for naval factory, can merge with corresponding cruiser method :)
            return Vector2.Distance(_camera.Transform.Position, bowSlot.Transform.position) < CAMERA_MARGIN_IN_M;
        }
    }
}