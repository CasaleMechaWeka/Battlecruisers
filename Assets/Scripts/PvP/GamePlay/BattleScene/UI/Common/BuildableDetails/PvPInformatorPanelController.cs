using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Panels;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public class PvPInformatorPanelController : PvPSlidingPanel, IPvPInformatorPanel
    {
        public PvPDismissInformatorButtonController dismissButton;

        public PvPBuildingDetailsController buildingDetails;
        public IPvPComparableItemDetails<IPvPBuilding> BuildingDetails => buildingDetails;

        public PvPUnitDetailsController unitDetails;
        public IPvPComparableItemDetails<IPvPUnit> UnitDetails => unitDetails;

        public PvPCruiserDetailsController cruiserDetails;
        public IPvPComparableItemDetails<IPvPCruiser> CruiserDetails => cruiserDetails;

        public PvPSlidingPanel informatorPanelExtended;
        public IPvPSlidingPanel ExtendedPanel => informatorPanelExtended;

        public PvPInformatorButtons buttons;
        public IPvPInformatorButtons Buttons => buttons;


        // public void Initialise(
        //     IPvPUIManager uiManager,
        //     IPvPCruiser playerCruiser,
        //     IPvPUserChosenTargetHelper userChosenTargetHelper,
        //     IPvPButtonVisibilityFilters visibilityFilters,
        //     IPvPSingleSoundPlayer soundPlayer)
        // {
        //     base.Initialise();
        //     PvPHelper.AssertIsNotNull(uiManager, playerCruiser, userChosenTargetHelper, visibilityFilters, soundPlayer);
        //     PvPHelper.AssertIsNotNull(informatorPanelExtended, buttons, buildingDetails, unitDetails, cruiserDetails);

        //     informatorPanelExtended.Initialise();
        //     buttons
        //         .Initialise(
        //             playerCruiser.DroneFocuser,
        //             playerCruiser.RepairManager,
        //             userChosenTargetHelper,
        //             visibilityFilters,
        //             soundPlayer,
        //             informatorPanelExtended,
        //             playerCruiser.FactoryProvider.UpdaterProvider.PerFrameUpdater,
        //             uiManager);

        //     buildingDetails.Initialise();
        //     unitDetails.Initialise();
        //     cruiserDetails.Initialise();
        //     dismissButton.Initialise(soundPlayer, uiManager, new PvPStaticBroadcastingFilter(isMatch: true));
        // }



        public void Initialise(
            IPvPUIManager uiManager,
            IPvPCruiser playerCruiser,
            IPvPUpdater perFrameUpdater,
            IPvPUserChosenTargetHelper userChosenTargetHelper,
            IPvPButtonVisibilityFilters visibilityFilters,
            IPvPSingleSoundPlayer soundPlayer)
        {
            base.Initialise();
            PvPHelper.AssertIsNotNull(uiManager, playerCruiser, visibilityFilters, soundPlayer);
            PvPHelper.AssertIsNotNull(informatorPanelExtended, buttons, buildingDetails, unitDetails, cruiserDetails);

            informatorPanelExtended.Initialise();
            buttons
                .Initialise(
                     playerCruiser.DroneFocuser,
                     playerCruiser.RepairManager,
                    userChosenTargetHelper,
                    visibilityFilters,
                    soundPlayer,
                    informatorPanelExtended,
                    perFrameUpdater,
                    uiManager);

            buildingDetails.Initialise();
            unitDetails.Initialise();
            cruiserDetails.Initialise();
            dismissButton.Initialise(soundPlayer, uiManager, new PvPStaticBroadcastingFilter(isMatch: true));
        }

        public override void Hide()
        {
            base.Hide();
            informatorPanelExtended.Hide();
        }

        public void Show(IPvPTarget item)
        {
            base.Show();

            Assert.IsNotNull(item);
            buttons.SelectedItem = item;
        }
    }
}