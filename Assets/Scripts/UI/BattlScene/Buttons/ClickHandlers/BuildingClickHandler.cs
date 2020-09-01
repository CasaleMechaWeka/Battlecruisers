using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Sound;
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
            _uiManager.SelectBuildingFromMenu(buildingClicked, canAffordBuildable);

            if (canAffordBuildable)
            {
                if (buildingClicked.Buildable.SlotSpecification.SlotType == SlotType.Bow
                    || buildingClicked.Buildable.SlotSpecification.BuildingFunction == BuildingFunction.AntiShip)
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
                PlayUnaffordableSound();
            }
        }
    }
}