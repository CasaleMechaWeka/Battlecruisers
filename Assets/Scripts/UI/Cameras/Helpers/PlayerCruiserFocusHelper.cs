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
        private readonly bool _isTutorial;

        public const float PLAYER_CRUISER_CAMERA_MARGIN_IN_M = 10;
        public const float BOW_SLOT_CAMERA_MARGIN_IN_M = 1;

        public PlayerCruiserFocusHelper(
            ICamera camera, 
            ICameraFocuser cameraFocuser, 
            ICruiser playerCruiser,
            bool isTutorial)
        {
            Helper.AssertIsNotNull(camera, cameraFocuser, playerCruiser);

            _camera = camera;
            _cameraFocuser = cameraFocuser;
            _playerCruiser = playerCruiser;
            _isTutorial = isTutorial;
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
            return Vector2.Distance(_camera.Position, _playerCruiser.Position) < PLAYER_CRUISER_CAMERA_MARGIN_IN_M;
        }

        public void FocusOnPlayerBowSlotIfNeeded()
        {
            if (!_isTutorial
                && !IsCameraRoughlyOnPlayerNavalFactory())
            {
                _cameraFocuser.FocusOnPlayerNavalFactory();
            }
        }

        private bool IsCameraRoughlyOnPlayerNavalFactory()
        {
            ISlot bowSlot 
                = _playerCruiser.SlotAccessor
                    .GetSlots(new SlotSpecification(SlotType.Bow))
                    .FirstOrDefault();

            Assert.IsNotNull(bowSlot);

            return Vector2.Distance(_camera.Position, bowSlot.Position) < BOW_SLOT_CAMERA_MARGIN_IN_M;
        }
    }
}