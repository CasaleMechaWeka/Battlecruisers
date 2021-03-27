using BattleCruisers.Buildables;
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
using BattleCruisers.Utils.Localisation;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    // FELIX  Rename to Parent?
    // FELIX  Delete informato button prefabs, only used in one place :)
    public class InformatorPanelController : SlidingPanel, IInformatorPanel
    {
        public DismissInformatorButtonController dismissButton;

        public BuildingDetailsController buildingDetails;
        public IComparableItemDetails<IBuilding> BuildingDetails => buildingDetails;

        public UnitDetailsController unitDetails;
        public IComparableItemDetails<IUnit> UnitDetails => unitDetails;

        public CruiserDetailsController cruiserDetails;
        public IComparableItemDetails<ICruiser> CruiserDetails => cruiserDetails;

        public SlidingPanel informatorPanel;

        public InformatorWidgetManager informatorWidgets;
        public IInformatorWidgetManager Widgets => informatorWidgets;

        public void Initialise(
            IUIManager uiManager,
            ICruiser playerCruiser,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters visibilityFilters,
            ISingleSoundPlayer soundPlayer,
            // FELIX  Remove? (After implementing item details :) )
            ILocTable commonStrings)
        {
            base.Initialise();
            Helper.AssertIsNotNull(uiManager, playerCruiser, userChosenTargetHelper, visibilityFilters, soundPlayer, commonStrings);
            Helper.AssertIsNotNull(informatorPanel, informatorWidgets, buildingDetails, unitDetails, cruiserDetails);

            informatorPanel.Initialise();
            informatorWidgets
                .Initialise(
                    playerCruiser.DroneFocuser, 
                    playerCruiser.RepairManager, 
                    userChosenTargetHelper, 
                    visibilityFilters, 
                    soundPlayer, 
                    informatorPanel,
                    playerCruiser.FactoryProvider.UpdaterProvider.PerFrameUpdater,
                    uiManager);

            buildingDetails.Initialise();
            unitDetails.Initialise();
            cruiserDetails.Initialise();
            dismissButton.Initialise(soundPlayer, uiManager, new StaticBroadcastingFilter(isMatch: true), visibilityFilters.HelpLabelsVisibilityFilter);
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