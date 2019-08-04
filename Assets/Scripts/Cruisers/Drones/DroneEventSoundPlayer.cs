using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Cruisers.Drones
{
    // FELIX  Update tests :)
    public class DroneEventSoundPlayer : IManagedDisposable
    {
        private readonly IDroneManagerMonitor _droneManagerMonitor;
        private readonly IPrioritisedSoundPlayer _soundPlayer;

        public DroneEventSoundPlayer(IDroneManagerMonitor droneManagerMonitor, IPrioritisedSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(droneManagerMonitor, soundPlayer);

            _droneManagerMonitor = droneManagerMonitor;
            _soundPlayer = soundPlayer;

            _droneManagerMonitor.DroneNumIncreased += _droneManagerMonitor_DroneNumIncreased;
            _droneManagerMonitor.IdleDronesStarted += _droneManagerMonitor_IdleDronesStarted;
        }

        private void _droneManagerMonitor_DroneNumIncreased(object sender, EventArgs e)
        {
            _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.Drones.NewDronesReady);
        }

        private void _droneManagerMonitor_IdleDronesStarted(object sender, EventArgs e)
        {
            _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.Drones.Idle);
        }

        public void DisposeManagedState()
        {
            _droneManagerMonitor.DroneNumIncreased -= _droneManagerMonitor_DroneNumIncreased;
            _droneManagerMonitor.IdleDronesStarted -= _droneManagerMonitor_IdleDronesStarted;
        }
    }
}