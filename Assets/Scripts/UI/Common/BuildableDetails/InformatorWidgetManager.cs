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
using BattleCruisers.Utils.Localisation;
using UnityEngine;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    // FELIX  Add all buttons
    // + Expand
    // + Delete
    // + Target
    // + Drone focus
    // + Repair

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
            // FELIX  Remove?
            ILocTable commonStrings,
            ISlidingPanel informatorPanel,
            IUpdater updater,
            IUIManager uiManager)
        {
            Helper.AssertIsNotNull(droneFocuser, repairManager, userChosenTargetHelper, buttonVisibilityFilters, soundPlayer, commonStrings, informatorPanel, updater, uiManager);
            Helper.AssertIsNotNull(extendButton, toggleDronesButton, chooseTargetButton, repairButton, deleteButton);

            extendButton.Initialise(soundPlayer, informatorPanel);
            toggleDronesButton.Initialise(soundPlayer);
            chooseTargetButton.Initialise(soundPlayer, userChosenTargetHelper, buttonVisibilityFilters.ChooseTargetButtonVisiblityFilter);
            repairButton.Initialise(soundPlayer, droneFocuser, repairManager);
            deleteButton.Initialise(soundPlayer, uiManager, buttonVisibilityFilters.DeletButtonVisiblityFilter, updater);

            // FELIX :D
            //_repairButton = GetComponentInChildren<RepairButtonController>(includeInactive: true);
            //Assert.IsNotNull(_repairButton);
            //_repairButton.Initialise(soundPlayer, droneFocuser, repairManager);

            //_toggleDronesButton = GetComponentInChildren<ToggleDroneButtonController>(includeInactive: true);
            //Assert.IsNotNull(_toggleDronesButton);
            //_toggleDronesButton.Initialise(soundPlayer);

            //_chooseTargetButton = GetComponentInChildren<ChooseTargetButtonController>(includeInactive: true);
            //Assert.IsNotNull(_chooseTargetButton);
            //_chooseTargetButton.Initialise(soundPlayer, userChosenTargetHelper, buttonVisibilityFilters.ChooseTargetButtonVisiblityFilter, commonStrings);
        }
    }
}
