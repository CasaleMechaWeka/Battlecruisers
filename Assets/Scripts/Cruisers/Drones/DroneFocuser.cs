using BattleCruisers.Buildables.Repairables;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones
{
    public class DroneFocuser : IDroneFocuser
    {
        private readonly IDroneManager _droneManager;
        private readonly IDroneFocusSoundPicker _soundPicker;
        private readonly IPrioritisedSoundPlayer _soundPlayer;

        public event EventHandler PlayerTriggeredRepair;

        public DroneFocuser(IDroneManager droneManager, IDroneFocusSoundPicker soundPicker, IPrioritisedSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(droneManager, soundPicker, soundPlayer);

            _droneManager = droneManager;
            _soundPicker = soundPicker;
            _soundPlayer = soundPlayer;
        }

        public void ToggleDroneConsumerFocus(IDroneConsumer droneConsumer, bool isTriggeredByPlayer)
        {
            Assert.IsNotNull(droneConsumer);

            DroneConsumerState preFocusState = droneConsumer.State;
            _droneManager.ToggleDroneConsumerFocus(droneConsumer);
            DroneConsumerState postFocusState = droneConsumer.State;

            if (isTriggeredByPlayer)
            {
                _soundPlayer.PlaySound(_soundPicker.PickSound(preFocusState, postFocusState));

                if (droneConsumer.NumOfDronesRequired == RepairManager.NUM_OF_DRONES_REQUIRED_FOR_REPAIR)
                {
                    PlayerTriggeredRepair?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}