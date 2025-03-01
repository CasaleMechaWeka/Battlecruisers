using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils.PlatformAbstractions;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers
{
    public class PvPPlayerCruiserFocusHelper : IPvPPlayerCruiserFocusHelper
    {
        private readonly ICamera _camera;
        private readonly ICameraFocuser _cameraFocuser;
        private readonly IPvPCruiser _playerCruiser;
        private readonly bool _isTutorial;

        public const float PLAYER_CRUISER_CAMERA_MARGIN_IN_M = 10;
        public const float BOW_SLOT_CAMERA_MARGIN_IN_M = 1;

        public PvPPlayerCruiserFocusHelper(
            ICamera camera,
            ICameraFocuser cameraFocuser,
            IPvPCruiser playerCruiser,
            bool isTutorial)
        {
            PvPHelper.AssertIsNotNull(camera, cameraFocuser, playerCruiser);

            _camera = camera;
            _cameraFocuser = cameraFocuser;
            _playerCruiser = playerCruiser;
            _isTutorial = isTutorial;
        }

        public void FocusOnPlayerCruiserIfNeeded()
        {
            if (!IsCameraRoughlyOnPlayerCruiser())
            {
                if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
                    _cameraFocuser.FocusOnLeftCruiser();
                else
                    _cameraFocuser.FocusOnRightCruiser();
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
                if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
                    _cameraFocuser.FocusOnLeftNavalFactory();
                else
                    _cameraFocuser.FocusOnRightNavalFactory();
            }
        }

        private bool IsCameraRoughlyOnPlayerNavalFactory()
        {
            IPvPSlot bowSlot
                = _playerCruiser.SlotAccessor
                    .GetSlots(new PvPSlotSpecification(SlotType.Bow))
                    .FirstOrDefault();

            Assert.IsNotNull(bowSlot);

            return Vector2.Distance(_camera.Position, bowSlot.Position) < BOW_SLOT_CAMERA_MARGIN_IN_M;
        }
    }
}