using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class RepairButtonController : MonoBehaviour
    {
        private Button _button;
        private IDroneManager _droneManager;
        private IRepairManager _repairManager;

        private ITarget _repairable;
		public ITarget Repairable
        {
            private get { return _repairable; }
            set
            {
                if (_repairable != null)
                {
                    _repairable.RepairCommand.CanExecuteChanged -= RepairCommand_CanExecuteChanged;
                }

                _repairable = value;

                if (_repairable != null)
                {
                    _repairable.RepairCommand.CanExecuteChanged += RepairCommand_CanExecuteChanged;
                    UpdateVisibility();
                }
            }
        }

        // Only show repair button for user repairables, not opponent repairables
        private bool ShowRepairButton { get { return Repairable.Faction == Faction.Blues && Repairable.RepairCommand.CanExecute; } }

        public void Initialise(IDroneManager droneManager, IRepairManager repairManager)
        {
            Helper.AssertIsNotNull(droneManager, repairManager);

            _droneManager = droneManager;
            _repairManager = repairManager;

            _button = GetComponent<Button>();
            Assert.IsNotNull(_button);
            _button.onClick.AddListener(ToggleRepairButton);
        }

        private void ToggleRepairButton()
        {
            IDroneConsumer repairDroneConsumer = _repairManager.GetDroneConsumer(Repairable);
            _droneManager.ToggleDroneConsumerFocus(repairDroneConsumer);
        }

        private void RepairCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
			gameObject.SetActive(ShowRepairButton);
        }
    }
}
