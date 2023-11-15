using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers
{
    public class PvPBuildingClickHandler : PvPBuildableClickHandler, IPvPBuildingClickHandler
    {
        private readonly IPvPPlayerCruiserFocusHelper _playerCruiserFocusHelper;
        private readonly IPvPAudioClipWrapper _buildingSelectedSound;

        public PvPBuildingClickHandler(
            IPvPUIManager uiManager,
            IPvPPrioritisedSoundPlayer eventSoundPlayer,
            IPvPSingleSoundPlayer uiSoundPlayer,
            IPvPPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPvPAudioClipWrapper buildingSelectedSound)
            : base(uiManager, eventSoundPlayer, uiSoundPlayer)
        {
            PvPHelper.AssertIsNotNull(playerCruiserFocusHelper, buildingSelectedSound);

            _playerCruiserFocusHelper = playerCruiserFocusHelper;
            _buildingSelectedSound = buildingSelectedSound;
        }

        public void HandleClick(bool canAffordBuildable, IPvPBuildableWrapper<IPvPBuilding> buildingClicked)
        {
            Assert.IsNotNull(buildingClicked);

            _uiSoundPlayer.PlaySound(_buildingSelectedSound);

            if (canAffordBuildable)
            {
                _uiManager.SelectBuilding(buildingClicked.Buildable);
                _uiManager.SelectBuildingFromMenu(buildingClicked);

                if (buildingClicked.Buildable.SlotSpecification.SlotType == PvPSlotType.Bow)
                {
                    _playerCruiserFocusHelper.FocusOnPlayerBowSlotIfNeeded();
                }
                else
                {
                    _playerCruiserFocusHelper.FocusOnPlayerCruiserIfNeeded();
                }
            }
            else
            {
                _uiManager.SelectBuilding(buildingClicked.Buildable);
                PlayUnaffordableSound();
            }
        }

        public void HandleHover(IPvPBuildableWrapper<IPvPBuilding> buildingClicked)
        {
            _uiManager.PeakBuildingDetails(buildingClicked.Buildable);
        }

        public void HandleHoverExit()
        {
            _uiManager.UnpeakBuildingDetails();
        }
    }
}