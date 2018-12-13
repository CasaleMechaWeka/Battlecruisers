using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class InformatorPanelController : Panel, IInformatorPanel
    {
        private DismissInformatorButtonController _dismissButton;

        private BuildingDetailsController _buildingDetails;
        public IBuildableDetails<IBuilding> BuildingDetails { get { return _buildingDetails; } }

        private UnitDetailsController _unitDetails;
        public IBuildableDetails<IUnit> UnitDetails { get { return _unitDetails; } }

        private CruiserDetailsController _cruiserDetails;
        public ICruiserDetails CruiserDetails { get { return _cruiserDetails; } }

        public void StaticInitialise()
        {
            _dismissButton = GetComponentInChildren<DismissInformatorButtonController>();
            Assert.IsNotNull(_dismissButton);

            _buildingDetails = GetComponentInChildren<BuildingDetailsController>(includeInactive: true);
            Assert.IsNotNull(_buildingDetails);

            _unitDetails = GetComponentInChildren<UnitDetailsController>(includeInactive: true);
            Assert.IsNotNull(_unitDetails);

            _cruiserDetails = GetComponentInChildren<CruiserDetailsController>(includeInactive: true);
            Assert.IsNotNull(_cruiserDetails);
        }

        // FELIX  Check if I can get rid of circular dependency and merge initialise methods?
        public void Initialise(
            IUIManager uiManager,
            ICruiser playerCruiser,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters visibilityFilters)
        {
            Helper.AssertIsNotNull(
                uiManager,
                playerCruiser,
                userChosenTargetHelper,
                visibilityFilters);

            // FELIX  Use new visibility filter (if tutorial still requires informator to not be dismissable sometimes :) )
            _dismissButton.Initialise(uiManager, new StaticBroadcastingFilter(isMatch: true));
            _buildingDetails.Initialise(playerCruiser.DroneFocuser, playerCruiser.RepairManager, userChosenTargetHelper, visibilityFilters.ChooseTargetButtonVisiblityFilter, visibilityFilters.DeletButtonVisiblityFilter);
            _unitDetails.Initialise(playerCruiser.DroneFocuser, playerCruiser.RepairManager, userChosenTargetHelper, visibilityFilters.ChooseTargetButtonVisiblityFilter, visibilityFilters.DeletButtonVisiblityFilter);
            _cruiserDetails.Initialise(playerCruiser.DroneFocuser, playerCruiser.RepairManager, userChosenTargetHelper, visibilityFilters.ChooseTargetButtonVisiblityFilter);
        }
    }
}