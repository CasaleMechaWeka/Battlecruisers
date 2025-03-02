using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Panels;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.BattleScene.Update;
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
        public ISlidingPanel ExtendedPanel => informatorPanelExtended;

        public PvPInformatorButtons buttons;
        public IInformatorButtons Buttons => buttons;

        public void Initialise(
            IPvPUIManager uiManager,
            IPvPCruiser playerCruiser,
            IUpdater perFrameUpdater,
            IUserChosenTargetHelper userChosenTargetHelper,
            IPvPButtonVisibilityFilters visibilityFilters,
            ISingleSoundPlayer soundPlayer)
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

        public void Show(ITarget item)
        {
            base.Show();

            Assert.IsNotNull(item);
            buttons.SelectedItem = item;
        }
    }
}