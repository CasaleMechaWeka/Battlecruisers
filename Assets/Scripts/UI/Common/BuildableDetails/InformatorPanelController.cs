using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class InformatorPanelController : SlidingPanel, IInformatorPanel
    {
        private DismissInformatorButtonController _dismissButton;

        private BuildingDetailsController _buildingDetails;
        public IBuildableDetails<IBuilding> BuildingDetails => _buildingDetails;

        private UnitDetailsController _unitDetails;
        public IBuildableDetails<IUnit> UnitDetails => _unitDetails;

        private CruiserDetailsController _cruiserDetails;
        public ICruiserDetails CruiserDetails => _cruiserDetails;

        public void Initialise(
            IUIManager uiManager,
            ICruiser playerCruiser,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters visibilityFilters,
            ISingleSoundPlayer soundPlayer)
        {
            base.Initialise();

            Helper.AssertIsNotNull(
                uiManager,
                playerCruiser,
                userChosenTargetHelper,
                visibilityFilters,
                soundPlayer);

            // Dismiss button
            _dismissButton = GetComponentInChildren<DismissInformatorButtonController>();
            Assert.IsNotNull(_dismissButton);
            _dismissButton.Initialise(soundPlayer, uiManager, new StaticBroadcastingFilter(isMatch: true), visibilityFilters.HelpLabelsVisibilityFilter);
            
            // Building details
            _buildingDetails = GetComponentInChildren<BuildingDetailsController>(includeInactive: true);
            Assert.IsNotNull(_buildingDetails);
            _buildingDetails.Initialise(uiManager, playerCruiser.DroneFocuser, playerCruiser.RepairManager, userChosenTargetHelper, visibilityFilters, soundPlayer);

            // Unit details
            _unitDetails = GetComponentInChildren<UnitDetailsController>(includeInactive: true);
            Assert.IsNotNull(_unitDetails);
            _unitDetails.Initialise(uiManager, playerCruiser.DroneFocuser, playerCruiser.RepairManager, userChosenTargetHelper, visibilityFilters, soundPlayer);

            // Cruiser details
            _cruiserDetails = GetComponentInChildren<CruiserDetailsController>(includeInactive: true);
            Assert.IsNotNull(_cruiserDetails);
            _cruiserDetails.Initialise(playerCruiser.DroneFocuser, playerCruiser.RepairManager, userChosenTargetHelper, visibilityFilters, soundPlayer);
        }
    }
}