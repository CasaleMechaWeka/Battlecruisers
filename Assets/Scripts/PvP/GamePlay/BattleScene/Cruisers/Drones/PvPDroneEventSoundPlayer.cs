using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public class PvPDroneEventSoundPlayer : IManagedDisposable
    {
        private readonly IDroneManagerMonitor _droneManagerMonitor;
        private readonly IPrioritisedSoundPlayer _soundPlayer;
        private readonly IDebouncer _idleDronesDebouncer;

        public PvPDroneEventSoundPlayer(IDroneManagerMonitor droneManagerMonitor, IPrioritisedSoundPlayer soundPlayer, IDebouncer idleDronesDebouncer)
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
            _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.Drones.NewDronesReady);
        }

        private void _droneManagerMonitor_IdleDronesStarted(object sender, EventArgs e)
        {
            _idleDronesDebouncer.Debounce(() => _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.Drones.Idle));
        }

        public void DisposeManagedState()
        {
            _droneManagerMonitor.DroneNumIncreased -= _droneManagerMonitor_DroneNumIncreased;
            _droneManagerMonitor.IdleDronesStarted -= _droneManagerMonitor_IdleDronesStarted;
        }
    }
}