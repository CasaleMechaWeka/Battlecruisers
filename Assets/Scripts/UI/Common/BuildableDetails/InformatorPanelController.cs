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
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class InformatorPanelController : SlidingPanel, IInformatorPanel
    {
        public DismissInformatorButtonController dismissButton;

        public BuildingDetailsController buildingDetails;
        public IComparableItemDetails<IBuilding> BuildingDetails => buildingDetails;

        public UnitDetailsController unitDetails;
        public IComparableItemDetails<IUnit> UnitDetails => unitDetails;

        public CruiserDetailsController cruiserDetails;
        public IComparableItemDetails<ICruiser> CruiserDetails => cruiserDetails;

        public SlidingPanel informatorPanelExtended;
        public ISlidingPanel ExtendedPanel => informatorPanelExtended;

        public InformatorButtons buttons;
        public IInformatorButtons Buttons => buttons;


        public void Initialise(
            IUIManager uiManager,
            ICruiser playerCruiser,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters visibilityFilters,
            ISingleSoundPlayer soundPlayer)
        {
            base.Initialise();
            Helper.AssertIsNotNull(uiManager, playerCruiser, userChosenTargetHelper, visibilityFilters, soundPlayer);
            Helper.AssertIsNotNull(informatorPanelExtended, buttons, buildingDetails, unitDetails, cruiserDetails);

            informatorPanelExtended.Initialise();
            buttons
                .Initialise(
                    playerCruiser.DroneFocuser, 
                    playerCruiser.RepairManager, 
                    userChosenTargetHelper, 
                    visibilityFilters, 
                    soundPlayer, 
                    informatorPanelExtended,
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
            informatorPanelExtended.Hide();
        }

        public void Show(ITarget item)
        {
            base.Show();

            Assert.IsNotNull(item);
            buttons.SelectedItem = item;
        }
    }
}