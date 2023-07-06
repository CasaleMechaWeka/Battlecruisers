using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Timers;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public class PvPDroneEventSoundPlayer : IPvPManagedDisposable
    {
        private readonly IPvPDroneManagerMonitor _droneManagerMonitor;
        private readonly IPvPPrioritisedSoundPlayer _soundPlayer;
        private readonly IPvPDebouncer _idleDronesDebouncer;

        public PvPDroneEventSoundPlayer(IPvPDroneManagerMonitor droneManagerMonitor, IPvPPrioritisedSoundPlayer soundPlayer, IPvPDebouncer idleDronesDebouncer)
        {
            PvPHelper.AssertIsNotNull(droneManagerMonitor, soundPlayer, idleDronesDebouncer);

            _droneManagerMonitor = droneManagerMonitor;
            _soundPlayer = soundPlayer;
            _idleDronesDebouncer = idleDronesDebouncer;

            _droneManagerMonitor.DroneNumIncreased += _droneManagerMonitor_DroneNumIncreased;
            _droneManagerMonitor.IdleDronesStarted += _droneManagerMonitor_IdleDronesStarted;
        }

        private void _droneManagerMonitor_DroneNumIncreased(object sender, EventArgs e)
        {
            _soundPlayer.PlaySound(PvPPrioritisedSoundKeys.PvPEvents.PvPDrones.NewDronesReady);
        }

        private void _droneManagerMonitor_IdleDronesStarted(object sender, EventArgs e)
        {
            _idleDronesDebouncer.Debounce(() => _soundPlayer.PlaySound(PvPPrioritisedSoundKeys.PvPEvents.PvPDrones.Idle));
        }

        public void DisposeManagedState()
        {
            _droneManagerMonitor.DroneNumIncreased -= _droneManagerMonitor_DroneNumIncreased;
            _droneManagerMonitor.IdleDronesStarted -= _droneManagerMonitor_IdleDronesStarted;
        }
    }
}