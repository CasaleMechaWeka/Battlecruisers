using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Cruisers.Drones
{
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
            _droneManagerMonitor.IdleDrones += _droneManagerMonitor_IdleDrones;
        }

        private void _droneManagerMonitor_DroneNumIncreased(object sender, EventArgs e)
        {
            _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.DronesNewDronesReady);
        }

        private void _droneManagerMonitor_IdleDrones(object sender, EventArgs e)
        {
            _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.DronesIdle);
        }

        public void DisposeManagedState()
        {
            _droneManagerMonitor.DroneNumIncreased -= _droneManagerMonitor_DroneNumIncreased;
            _droneManagerMonitor.IdleDrones -= _droneManagerMonitor_IdleDrones;
        }
    }
}