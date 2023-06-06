using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Panels;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public class PvPInformatorButtons : MonoBehaviour, IPvPInformatorButtons
    {
        public PvPRepairButtonController repairButton;
        public PvPChooseTargetButtonController chooseTargetButton;
        public PvPDeleteButtonController deleteButton;
        public PvPExtendInformatorButtonController extendButton;

        public PvPToggleDroneButtonController toggleDronesButton;
        public IPvPButton ToggleDronesButton => toggleDronesButton;

        public IPvPTarget SelectedItem
        {
            set
            {
                repairButton.Repairable = value;
                chooseTargetButton.Target = value;

                // Ok to not be buildable (eg: ICruiser)
                IPvPBuildable buildable = value as IPvPBuildable;
                toggleDronesButton.Buildable = buildable;
                deleteButton.Buildable = buildable;
            }
        }

        public void Initialise(
            IPvPDroneFocuser droneFocuser,
            IPvPRepairManager repairManager,
            IPvPUserChosenTargetHelper userChosenTargetHelper,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IPvPSingleSoundPlayer soundPlayer,
            IPvPSlidingPanel informatorPanel,
            IPvPUpdater updater,
            IPvPUIManager uiManager)
        {
            PvPHelper.AssertIsNotNull(droneFocuser, repairManager, userChosenTargetHelper, buttonVisibilityFilters, soundPlayer, informatorPanel, updater, uiManager);
            PvPHelper.AssertIsNotNull(extendButton, toggleDronesButton, chooseTargetButton, repairButton, deleteButton);

            extendButton.Initialise(soundPlayer, informatorPanel);
            toggleDronesButton.Initialise(soundPlayer);
            chooseTargetButton.Initialise(soundPlayer, userChosenTargetHelper, buttonVisibilityFilters.ChooseTargetButtonVisiblityFilter);
            repairButton.Initialise(soundPlayer, droneFocuser, repairManager);
            deleteButton.Initialise(soundPlayer, uiManager, buttonVisibilityFilters.DeletButtonVisiblityFilter, updater);
        }

        public void Initialise(
            // IPvPDroneFocuser droneFocuser,
            // IPvPRepairManager repairManager,
            // IPvPUserChosenTargetHelper userChosenTargetHelper,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IPvPSingleSoundPlayer soundPlayer,
            IPvPSlidingPanel informatorPanel,
            IPvPUpdater updater,
            IPvPUIManager uiManager)
        {
            PvPHelper.AssertIsNotNull(buttonVisibilityFilters, soundPlayer, informatorPanel, updater, uiManager);
            PvPHelper.AssertIsNotNull(extendButton, toggleDronesButton, chooseTargetButton, repairButton, deleteButton);

            extendButton.Initialise(soundPlayer, informatorPanel);
            toggleDronesButton.Initialise(soundPlayer);
            chooseTargetButton.Initialise(soundPlayer, buttonVisibilityFilters.ChooseTargetButtonVisiblityFilter);
            // repairButton.Initialise(soundPlayer, droneFocuser, repairManager);
            deleteButton.Initialise(soundPlayer, uiManager, buttonVisibilityFilters.DeletButtonVisiblityFilter, updater);
        }

        public void Initialise(
            // IPvPDroneFocuser droneFocuser,
            // IPvPRepairManager repairManager,
            // IPvPUserChosenTargetHelper userChosenTargetHelper,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IPvPSingleSoundPlayer soundPlayer,
            IPvPSlidingPanel informatorPanel,
            // IPvPUpdater updater,
            IPvPUIManager uiManager)
        {
            PvPHelper.AssertIsNotNull(buttonVisibilityFilters, soundPlayer, informatorPanel, uiManager);
            PvPHelper.AssertIsNotNull(extendButton, toggleDronesButton, chooseTargetButton, repairButton, deleteButton);

            // extendButton.Initialise(soundPlayer, informatorPanel);
            // toggleDronesButton.Initialise(soundPlayer);
            // chooseTargetButton.Initialise(soundPlayer, buttonVisibilityFilters.ChooseTargetButtonVisiblityFilter);
            // repairButton.Initialise(soundPlayer, droneFocuser, repairManager);
            // deleteButton.Initialise(soundPlayer, uiManager, buttonVisibilityFilters.DeletButtonVisiblityFilter, updater);
        }
    }
}