using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using UnityEngine;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    // FELIX  Remove buttons from all item detail prefabs :)
    // FELIX  Rename? Just has buttons?
    public class InformatorWidgetManager : MonoBehaviour, IInformatorWidgetManager
    {
        public RepairButtonController repairButton;
        public ChooseTargetButtonController chooseTargetButton;
        public DeleteButtonController deleteButton;

        public ToggleDroneButtonController toggleDronesButton;
        public IButton ToggleDronesButton => toggleDronesButton;

        public ExtendInformatorButtonController extendButton;

        public ITarget SelectedItem
        {
            set
            {
                repairButton.Repairable = value;
                chooseTargetButton.Target = value;

                // Ok to not be buildable (eg: ICruiser)
                IBuildable buildable = value as IBuildable;
                toggleDronesButton.Buildable = buildable;
                deleteButton.Buildable = buildable;
            }
        }

        public void Initialise(
            IDroneFocuser droneFocuser,
            IRepairManager repairManager,
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters buttonVisibilityFilters,
            ISingleSoundPlayer soundPlayer,
            ISlidingPanel informatorPanel,
            IUpdater updater,
            IUIManager uiManager)
        {
            Helper.AssertIsNotNull(droneFocuser, repairManager, userChosenTargetHelper, buttonVisibilityFilters, soundPlayer, informatorPanel, updater, uiManager);
            Helper.AssertIsNotNull(extendButton, toggleDronesButton, chooseTargetButton, repairButton, deleteButton);

            extendButton.Initialise(soundPlayer, informatorPanel);
            toggleDronesButton.Initialise(soundPlayer);
            chooseTargetButton.Initialise(soundPlayer, userChosenTargetHelper, buttonVisibilityFilters.ChooseTargetButtonVisiblityFilter);
            repairButton.Initialise(soundPlayer, droneFocuser, repairManager);
            deleteButton.Initialise(soundPlayer, uiManager, buttonVisibilityFilters.DeletButtonVisiblityFilter, updater);
        }
    }
}