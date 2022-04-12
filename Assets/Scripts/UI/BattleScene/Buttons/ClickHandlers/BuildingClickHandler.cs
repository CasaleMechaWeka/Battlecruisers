using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons.ClickHandlers
{
    public class BuildingClickHandler : BuildableClickHandler, IBuildingClickHandler
    {
        private readonly IPlayerCruiserFocusHelper _playerCruiserFocusHelper;
        private readonly IAudioClipWrapper _buildingSelectedSound;

        public BuildingClickHandler(
            IUIManager uiManager, 
            IPrioritisedSoundPlayer eventSoundPlayer,
            ISingleSoundPlayer uiSoundPlayer,
            IPlayerCruiserFocusHelper playerCruiserFocusHelper, 
            IAudioClipWrapper buildingSelectedSound)
            : base(uiManager, eventSoundPlayer, uiSoundPlayer)
        {
            Helper.AssertIsNotNull(playerCruiserFocusHelper, buildingSelectedSound);

            _playerCruiserFocusHelper = playerCruiserFocusHelper;
            _buildingSelectedSound = buildingSelectedSound;
        }

        public void HandleClick(bool canAffordBuildable, IBuildableWrapper<IBuilding> buildingClicked)
        {
            Assert.IsNotNull(buildingClicked);

            _uiSoundPlayer.PlaySound(_buildingSelectedSound);

            if (canAffordBuildable)
            {
                _uiManager.SelectBuilding(buildingClicked.Buildable);
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
                //_uiManager.SelectBuilding(buildingClicked.Buildable);
                PlayUnaffordableSound();
            }
        }

        public void HandleHover(IBuildableWrapper<IBuilding> buildingClicked)
        {
            _uiManager.PeakBuildingDetails(buildingClicked.Buildable);
        }

        public void HandleHoverExit()
        {
            _uiManager.UnpeakBuildingDetails();
        }
    }
}