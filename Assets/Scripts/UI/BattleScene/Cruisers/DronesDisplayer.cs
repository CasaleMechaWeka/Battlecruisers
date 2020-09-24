using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;

namespace BattleCruisers.UI.BattleScene.Cruisers
{
    public class DronesDisplayer
    {
        private readonly IDroneManager _droneManager;
        private readonly IDroneManagerMonitor _droneManagerMonitor;
        private readonly INumberDisplay _numberDisplay;
        private readonly IGameObject _idleFeedback;

        public DronesDisplayer(
            IDroneManager droneManager, 
            IDroneManagerMonitor droneManagerMonitor, 
            INumberDisplay numberDisplay, 
            IGameObject idleFeedback)
        {
            Helper.AssertIsNotNull(droneManager, droneManagerMonitor, numberDisplay, idleFeedback);

            _droneManager = droneManager;
            _droneManagerMonitor = droneManagerMonitor;
            _numberDisplay = numberDisplay;
            _idleFeedback = idleFeedback;

            _numberDisplay.Num = _droneManager.NumOfDrones;

            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
            _droneManagerMonitor.IdleDronesStarted += _droneManagerMonitor_IdleDronesStarted;
            _droneManagerMonitor.IdleDronesEnded += _droneManagerMonitor_IdleDronesEnded;
        }

        private void _droneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            _numberDisplay.Num = _droneManager.NumOfDrones;
        }

        private void _droneManagerMonitor_IdleDronesStarted(object sender, EventArgs e)
        {
            _idleFeedback.IsVisible = true;
        }

        private void _droneManagerMonitor_IdleDronesEnded(object sender, EventArgs e)
        {
            _idleFeedback.IsVisible = false;
        }
    }
}