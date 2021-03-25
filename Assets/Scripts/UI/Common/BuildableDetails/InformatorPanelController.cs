using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using NSubstitute;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    // FELIX  Rename to Parent?
    public class InformatorPanelController : SlidingPanel, IInformatorPanel
    {
        private DismissInformatorButtonController _dismissButton;

        // FELIX
        private BuildingDetailsController _buildingDetails;
        public IBuildableDetails<IBuilding> BuildingDetails { get; private set; }
        //public IBuildableDetails<IBuilding> BuildingDetails => _buildingDetails;

        private UnitDetailsController _unitDetails;
        public IBuildableDetails<IUnit> UnitDetails { get; private set; }
        //public IBuildableDetails<IUnit> UnitDetails => _unitDetails;

        private CruiserDetailsController _cruiserDetails;
        public ICruiserDetails CruiserDetails { get; private set; }
        //public ICruiserDetails CruiserDetails => _cruiserDetails;

        public SlidingPanel informatorPanel;
        public InformatorWidgetManager informatorWidgets;

        public void Initialise(
            IUIManager uiManager,
            ICruiser playerCruiser,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters visibilityFilters,
            ISingleSoundPlayer soundPlayer,
            ILocTable commonStrings)
        {
            base.Initialise();
            Helper.AssertIsNotNull(uiManager, playerCruiser, userChosenTargetHelper, visibilityFilters, soundPlayer, commonStrings);
            Helper.AssertIsNotNull(informatorPanel, informatorWidgets);

            informatorPanel.Initialise();
            informatorWidgets.Initialise(playerCruiser.DroneFocuser, playerCruiser.RepairManager, userChosenTargetHelper, visibilityFilters, soundPlayer, commonStrings, informatorPanel);

            // FELIX  TEMP :P
            BuildingDetails = Substitute.For<IBuildableDetails<IBuilding>>();
            UnitDetails = Substitute.For<IBuildableDetails<IUnit>>();
            CruiserDetails = Substitute.For<ICruiserDetails>();

            //// Dismiss button
            //_dismissButton = GetComponentInChildren<DismissInformatorButtonController>();
            //Assert.IsNotNull(_dismissButton);
            //_dismissButton.Initialise(soundPlayer, uiManager, new StaticBroadcastingFilter(isMatch: true), visibilityFilters.HelpLabelsVisibilityFilter);

            //// Building details
            //_buildingDetails = GetComponentInChildren<BuildingDetailsController>(includeInactive: true);
            //Assert.IsNotNull(_buildingDetails);
            //_buildingDetails.Initialise(uiManager, playerCruiser.DroneFocuser, playerCruiser.RepairManager, userChosenTargetHelper, visibilityFilters, soundPlayer, commonStrings);

            //// Unit details
            //_unitDetails = GetComponentInChildren<UnitDetailsController>(includeInactive: true);
            //Assert.IsNotNull(_unitDetails);
            //_unitDetails.Initialise(uiManager, playerCruiser.DroneFocuser, playerCruiser.RepairManager, userChosenTargetHelper, visibilityFilters, soundPlayer, commonStrings);

            //// Cruiser details
            //_cruiserDetails = GetComponentInChildren<CruiserDetailsController>(includeInactive: true);
            //Assert.IsNotNull(_cruiserDetails);
            //_cruiserDetails.Initialise(playerCruiser.DroneFocuser, playerCruiser.RepairManager, userChosenTargetHelper, visibilityFilters, soundPlayer, commonStrings);
        }

        public override void Hide()
        {
            base.Hide();
            informatorPanel.Hide();
        }

        public void Show(ITarget item)
        {
            base.Show();

            Assert.IsNotNull(item);
            informatorWidgets.SelectedItem = item;
        }
    }
}