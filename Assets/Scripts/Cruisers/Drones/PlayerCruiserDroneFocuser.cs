using BattleCruisers.Buildables.Repairables;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;
using UnityEngine;

namespace BattleCruisers.Cruisers.Drones
{
    public class PlayerCruiserDroneFocuser : IDroneFocuser
    {
        private readonly IDroneManager _droneManager;
        private readonly IDroneFocusSoundPicker _soundPicker;
        private readonly IPrioritisedSoundPlayer _soundPlayer;

        public event EventHandler PlayerTriggeredRepair;

        public PlayerCruiserDroneFocuser(IDroneManager droneManager, IDroneFocusSoundPicker soundPicker, IPrioritisedSoundPlayer soundPlayer)
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
                PrioritisedSoundKey sound =  _soundPicker.PickSound(preFocusState, postFocusState);
                //Debug.Log(sound.Key);
                _soundPlayer.PlaySound(sound);

                if (droneConsumer.NumOfDronesRequired == RepairManager.NUM_OF_DRONES_REQUIRED_FOR_REPAIR)
                {
                    PlayerTriggeredRepair?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}