using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class ButtonManager : MonoBehaviour, IButtonManager
    {
        private RepairButtonController _repairButton;
        private ChooseTargetButtonController _chooseTargetButton;
        private DeleteButtonController _deleteButton;

        private ToggleDroneButtonController _toggleDronesButton;
        public IButton ToggleDronesButton { get { return _toggleDronesButton; } }

        public IBuildable Buildable
        {
            set 
            {
                IBuildable buildable = value;

                // If the buildable is not initialised, it does not exist in the
                // scene yet and this bottom bar does not need to be displayed.
                IsVisible = buildable != null && buildable.IsInitialised;

                _toggleDronesButton.Buildable = buildable;
                _repairButton.Repairable = buildable;
                _chooseTargetButton.Target = buildable;
            }
        }

        private bool IsVisible 
        { 
            get { return gameObject.activeSelf; } 
            set { gameObject.SetActive(value); }
        }

        public void Initialise(
            IDroneFocuser droneFocuser, 
            IRepairManager repairManager, 
            IUserChosenTargetHelper userChosenTargetHelper,
            IFilter<ITarget> chooseTargetButtonVisibilityFilter)
        {
            Helper.AssertIsNotNull(droneFocuser, repairManager, userChosenTargetHelper, chooseTargetButtonVisibilityFilter);

            _repairButton = GetComponentInChildren<RepairButtonController>(includeInactive: true);
            Assert.IsNotNull(_repairButton);
            _repairButton.Initialise(droneFocuser, repairManager);

            _toggleDronesButton = GetComponentInChildren<ToggleDroneButtonController>(includeInactive: true);
            Assert.IsNotNull(_toggleDronesButton);
            _toggleDronesButton.Initialise();

            _chooseTargetButton = GetComponentInChildren<ChooseTargetButtonController>(includeInactive: true);
            Assert.IsNotNull(_chooseTargetButton);
            _chooseTargetButton.Initialise(userChosenTargetHelper, chooseTargetButtonVisibilityFilter);
        }
    }
}
