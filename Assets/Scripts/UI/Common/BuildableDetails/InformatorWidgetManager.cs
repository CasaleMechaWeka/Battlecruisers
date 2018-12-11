using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class InformatorWidgetManager : MonoBehaviour, IInformatorWidgetManager
    {
        private RepairButtonController _repairButton;
        private ChooseTargetButtonController _chooseTargetButton;
        private DeleteButtonController _deleteButton;
        private IHealthDial<IBuildable> _healthDial;

        private ToggleDroneButtonController _toggleDronesButton;
        public IButton ToggleDronesButton { get { return _toggleDronesButton; } }

        public IBuildable Buildable
        {
            set 
            {
                _toggleDronesButton.Buildable = value;
                _repairButton.Repairable = value;
                _chooseTargetButton.Target = value;
                _healthDial.Damagable = value;
            }
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

            BuildableHealthDialInitialiser healthDialInitialiser = GetComponentInChildren<BuildableHealthDialInitialiser>(includeInactive: true);
            Assert.IsNotNull(healthDialInitialiser);
            _healthDial = healthDialInitialiser.Initialise();
        }
    }
}
