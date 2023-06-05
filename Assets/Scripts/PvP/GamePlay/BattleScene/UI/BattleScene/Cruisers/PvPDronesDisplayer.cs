using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Cruisers
{
    public class PvPDronesDisplayer
    {
        private readonly IPvPDroneManager _droneManager;
        private readonly IPvPDroneManagerMonitor _droneManagerMonitor;
        private readonly IPvPNumberDisplay _numberDisplay;
        private readonly IPvPGameObject _idleFeedback;

        public PvPDronesDisplayer(
            IPvPDroneManager droneManager,
            IPvPDroneManagerMonitor droneManagerMonitor,
            IPvPNumberDisplay numberDisplay,
            IPvPGameObject idleFeedback)
        {
            PvPHelper.AssertIsNotNull(droneManager, droneManagerMonitor, numberDisplay, idleFeedback);

            _droneManager = droneManager;
            _droneManagerMonitor = droneManagerMonitor;
            _numberDisplay = numberDisplay;
            _idleFeedback = idleFeedback;

            _numberDisplay.Num = _droneManager.NumOfDrones;

            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
            _droneManagerMonitor.IdleDronesStarted += _droneManagerMonitor_IdleDronesStarted;
            _droneManagerMonitor.IdleDronesEnded += _droneManagerMonitor_IdleDronesEnded;
        }


        public PvPDronesDisplayer(
            IPvPNumberDisplay numberDisplay,
            IPvPGameObject idleFeedback)
        {
            PvPHelper.AssertIsNotNull(numberDisplay, idleFeedback);

            // _droneManager = droneManager;
            // _droneManagerMonitor = droneManagerMonitor;
            _numberDisplay = numberDisplay;
            _idleFeedback = idleFeedback;

            // _numberDisplay.Num = _droneManager.NumOfDrones;

            // _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
            // _droneManagerMonitor.IdleDronesStarted += _droneManagerMonitor_IdleDronesStarted;
            // _droneManagerMonitor.IdleDronesEnded += _droneManagerMonitor_IdleDronesEnded;
        }

        private void _droneManager_DroneNumChanged(object sender, PvPDroneNumChangedEventArgs e)
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