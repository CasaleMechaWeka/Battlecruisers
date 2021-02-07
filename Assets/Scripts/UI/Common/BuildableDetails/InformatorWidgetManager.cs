using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class InformatorWidgetManager : MonoBehaviour, IInformatorWidgetManager
    {
        private RepairButtonController _repairButton;
        private ChooseTargetButtonController _chooseTargetButton;

        private ToggleDroneButtonController _toggleDronesButton;
        public IButton ToggleDronesButton => _toggleDronesButton;

        public IBuildable Buildable
        {
            set 
            {
                _toggleDronesButton.Buildable = value;
                _repairButton.Repairable = value;
                _chooseTargetButton.Target = value;
            }
        }

        public void Initialise(
            IDroneFocuser droneFocuser, 
            IRepairManager repairManager, 
            IUserChosenTargetHelper userChosenTargetHelper,
            IButtonVisibilityFilters buttonVisibilityFilters,
            ISingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(droneFocuser, repairManager, userChosenTargetHelper, buttonVisibilityFilters, soundPlayer);

            _repairButton = GetComponentInChildren<RepairButtonController>(includeInactive: true);
            Assert.IsNotNull(_repairButton);
            _repairButton.Initialise(soundPlayer, droneFocuser, repairManager);

            _toggleDronesButton = GetComponentInChildren<ToggleDroneButtonController>(includeInactive: true);
            Assert.IsNotNull(_toggleDronesButton);
            _toggleDronesButton.Initialise(soundPlayer);

            _chooseTargetButton = GetComponentInChildren<ChooseTargetButtonController>(includeInactive: true);
            Assert.IsNotNull(_chooseTargetButton);
            _chooseTargetButton.Initialise(soundPlayer, userChosenTargetHelper, buttonVisibilityFilters.ChooseTargetButtonVisiblityFilter);
        }
    }
}
