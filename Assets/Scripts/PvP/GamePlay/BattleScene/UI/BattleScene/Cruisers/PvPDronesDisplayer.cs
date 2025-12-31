using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Cruisers
{
    public class PvPDronesDisplayer
    {
        private readonly DroneManager _droneManager;
        private readonly DroneManagerMonitor _droneManagerMonitor;
        private readonly INumberDisplay _numberDisplay;
        private readonly IGameObject _idleFeedback;
        private PvPCruiser _playerCruiser;

        public PvPDronesDisplayer(
            DroneManager droneManager,
            DroneManagerMonitor droneManagerMonitor,
            INumberDisplay numberDisplay,
            IGameObject idleFeedback)
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
            INumberDisplay numberDisplay,
            IGameObject idleFeedback)
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