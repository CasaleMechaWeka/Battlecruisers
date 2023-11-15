using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers
{
    public class PvPPlayerCruiserFocusHelper : IPvPPlayerCruiserFocusHelper
    {
        private readonly IPvPCamera _camera;
        private readonly IPvPCameraFocuser _cameraFocuser;
        private readonly IPvPCruiser _playerCruiser;
        private readonly bool _isTutorial;

        public const float PLAYER_CRUISER_CAMERA_MARGIN_IN_M = 10;
        public const float BOW_SLOT_CAMERA_MARGIN_IN_M = 1;

        public PvPPlayerCruiserFocusHelper(
            IPvPCamera camera,
            IPvPCameraFocuser cameraFocuser,
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
                    _cameraFocuser.FocusOnLeftPlayerCruiser();
                else
                    _cameraFocuser.FocusOnRightPlayerCruiser();
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
                    _cameraFocuser.FocusOnLeftPlayerNavalFactory();
                else
                    _cameraFocuser.FocusOnRightPlayerNavalFactory();
            }
        }

        private bool IsCameraRoughlyOnPlayerNavalFactory()
        {
            IPvPSlot bowSlot
                = _playerCruiser.SlotAccessor
                    .GetSlots(new PvPSlotSpecification(PvPSlotType.Bow))
                    .FirstOrDefault();

            Assert.IsNotNull(bowSlot);

            return Vector2.Distance(_camera.Position, bowSlot.Position) < BOW_SLOT_CAMERA_MARGIN_IN_M;
        }
    }
}