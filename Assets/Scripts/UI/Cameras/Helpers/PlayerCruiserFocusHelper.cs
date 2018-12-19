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
    public class PlayerCruiserFocusHelper : IPlayerCruiserFocusHelper
    {
        private readonly ICamera _camera;
        private readonly ICameraFocuser _cameraFocuser;
        private readonly ICruiser _playerCruiser;

        public const float PLAYER_CRUISER_CAMERA_MARGIN_IN_M = 10;
        public const float BOW_SLOT_CAMERA_MARGIN_IN_M = 3;

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
            return Vector2.Distance(_camera.Transform.Position, _playerCruiser.Position) < PLAYER_CRUISER_CAMERA_MARGIN_IN_M;
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

            return Vector2.Distance(_camera.Transform.Position, bowSlot.Position) < BOW_SLOT_CAMERA_MARGIN_IN_M;
        }
    }
}