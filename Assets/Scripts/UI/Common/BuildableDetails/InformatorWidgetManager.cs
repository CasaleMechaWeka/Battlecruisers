using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
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
        private RepairButtonController _repairButton;
        public ChooseTargetButtonController chooseTargetButton;

        public ToggleDroneButtonController toggleDronesButton;
        public IButton ToggleDronesButton => toggleDronesButton;

        public ExtendInformatorButtonController extendButton;

        public ITarget SelectedItem
        {
            set 
            {
                toggleDronesButton.Buildable = value as IBuildable;
                // FELIX  Fix :P
                //_repairButton.Repairable = value;
                chooseTargetButton.Target = value;
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
            ISlidingPanel informatorPanel)
        {
            Helper.AssertIsNotNull(droneFocuser, repairManager, userChosenTargetHelper, buttonVisibilityFilters, soundPlayer, commonStrings, informatorPanel);
            Helper.AssertIsNotNull(extendButton, toggleDronesButton, chooseTargetButton);

            extendButton.Initialise(soundPlayer, informatorPanel);
            toggleDronesButton.Initialise(soundPlayer);
            chooseTargetButton.Initialise(soundPlayer, userChosenTargetHelper, buttonVisibilityFilters.ChooseTargetButtonVisiblityFilter);

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
