using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class RepairButtonController : CanvasGroupButton
    {
        private IDroneFocuser _droneFocuser;
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
        private bool ShowRepairButton 
        { 
            get 
            { 
                return 
                    Repairable.Faction == Faction.Blues 
                    && Repairable.RepairCommand.CanExecute; 
            } 
        }

        public void Initialise(IDroneFocuser droneFocuser, IRepairManager repairManager)
        {
            base.Initialise();

            Helper.AssertIsNotNull(droneFocuser, repairManager);

            _droneFocuser = droneFocuser;
            _repairManager = repairManager;
        }

        protected override void OnClicked()
        {
            IDroneConsumer repairDroneConsumer = _repairManager.GetDroneConsumer(Repairable);
            _droneFocuser.ToggleDroneConsumerFocus(repairDroneConsumer, isTriggeredByPlayer: true);
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
