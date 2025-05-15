using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers
{
    public class PvPBuildingClickHandler : PvPBuildableClickHandler, IPvPBuildingClickHandler
    {
        private readonly IPlayerCruiserFocusHelper _playerCruiserFocusHelper;
        private readonly AudioClipWrapper _buildingSelectedSound;

        public PvPBuildingClickHandler(
            PvPUIManager uiManager,
            IPrioritisedSoundPlayer eventSoundPlayer,
            SingleSoundPlayer uiSoundPlayer,
            IPlayerCruiserFocusHelper playerCruiserFocusHelper,
            AudioClipWrapper buildingSelectedSound)
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
            _uiManager.SelectBuilding(buildingClicked.Buildable);
            if (canAffordBuildable)
            {
                // _uiManager.SelectBuilding(buildingClicked.Buildable);
                _uiManager.SelectBuildingFromMenu(buildingClicked);

                if (buildingClicked.Buildable.SlotSpecification.SlotType == SlotType.Bow)
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
                // _uiManager.SelectBuilding(buildingClicked.Buildable);
                _uiManager.HideSlotsIfCannotAffordable();
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