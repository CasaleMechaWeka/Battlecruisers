using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Sound;
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public class PvPPlayerCruiserDroneFocuser : IPvPDroneFocuser
    {
        private readonly IDroneManager _droneManager;
        private readonly IDroneFocusSoundPicker _soundPicker;
        // private readonly IPrioritisedSoundPlayer _soundPlayer;

        public event EventHandler PlayerTriggeredRepair;

        public PvPPlayerCruiserDroneFocuser(IDroneManager droneManager, IDroneFocusSoundPicker soundPicker /* , IPrioritisedSoundPlayer soundPlayer */)
        {
            PvPHelper.AssertIsNotNull(droneManager, soundPicker /*, soundPlayer */);

            _droneManager = droneManager;
            _soundPicker = soundPicker;
            // _soundPlayer = soundPlayer;
        }

        public PrioritisedSoundKey ToggleDroneConsumerFocus(IDroneConsumer droneConsumer, bool isTriggeredByPlayer)
        {
            //  Assert.IsNotNull(droneConsumer);
            if (droneConsumer == null)
                return null;

            DroneConsumerState preFocusState = droneConsumer.State;
            _droneManager.ToggleDroneConsumerFocus(droneConsumer);
            DroneConsumerState postFocusState = droneConsumer.State;

            if (isTriggeredByPlayer)
            {
                try
                {
                    PrioritisedSoundKey sound = _soundPicker.PickSound(preFocusState, postFocusState);
                    // Debug.Log(sound.Key);
                    // _soundPlayer.PlaySound(sound);

                    if (droneConsumer.NumOfDronesRequired == PvPRepairManager.NUM_OF_DRONES_REQUIRED_FOR_REPAIR)
                    {
                        PlayerTriggeredRepair?.Invoke(this, EventArgs.Empty);
                    }

                    return sound;
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }

            }

            return null;
        }
    }
}