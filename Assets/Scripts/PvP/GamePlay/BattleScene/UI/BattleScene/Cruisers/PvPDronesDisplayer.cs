using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.UI.BattleScene.Cruisers;
using System;
using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Cruisers
{
    public class PvPDronesDisplayer
    {
        private readonly IPvPDroneManager _droneManager;
        private readonly IPvPDroneManagerMonitor _droneManagerMonitor;
        private readonly IPvPNumberDisplay _numberDisplay;
        private readonly IPvPGameObject _idleFeedback;
        private PvPCruiser _playerCruiser;

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

/*            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
            _droneManagerMonitor.IdleDronesStarted += _droneManagerMonitor_IdleDronesStarted;
            _droneManagerMonitor.IdleDronesEnded += _droneManagerMonitor_IdleDronesEnded;*/
        }


        public PvPDronesDisplayer(
            PvPCruiser playerCruiser,
            IPvPNumberDisplay numberDisplay,
            IPvPGameObject idleFeedback)
        {
            PvPHelper.AssertIsNotNull(numberDisplay, idleFeedback);

            // _droneManager = droneManager;
            // _droneManagerMonitor = droneManagerMonitor;
            _playerCruiser = playerCruiser;
            _numberDisplay = numberDisplay;
            _idleFeedback = idleFeedback;
            _numberDisplay.Num = playerCruiser.pvp_NumOfDrones.Value;

            // _numberDisplay.Num = _droneManager.NumOfDrones;
            playerCruiser.pvp_NumOfDrones.OnValueChanged += _droneManager_DroneNumChanged;
            playerCruiser.pvp_IdleDronesStarted.OnValueChanged += _droneManagerMonitor_IdleDronesStarted;
            playerCruiser.pvp_IdleDronesEnded.OnValueChanged += _droneManagerMonitor_IdleDronesEnded;
            //   _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
            // _droneManagerMonitor.IdleDronesStarted += _droneManagerMonitor_IdleDronesStarted;
            // _droneManagerMonitor.IdleDronesEnded += _droneManagerMonitor_IdleDronesEnded;
        }

        private void _droneManager_DroneNumChanged(int previous, int current)
        {
            _numberDisplay.Num = current;
        }

        private void _droneManagerMonitor_IdleDronesStarted(bool previous, bool current)
        {
            _idleFeedback.IsVisible = true;
        }

        private void _droneManagerMonitor_IdleDronesEnded(bool previous, bool current)
        {
            _idleFeedback.IsVisible = false;
        }
    }
}