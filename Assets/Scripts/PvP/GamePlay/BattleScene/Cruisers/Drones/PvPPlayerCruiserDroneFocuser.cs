using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using UnityEngine.Assertions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public class PvPPlayerCruiserDroneFocuser : IPvPDroneFocuser
    {
        private readonly IPvPDroneManager _droneManager;
        private readonly IPvPDroneFocusSoundPicker _soundPicker;
        // private readonly IPvPPrioritisedSoundPlayer _soundPlayer;

        public event EventHandler PlayerTriggeredRepair;

        public PvPPlayerCruiserDroneFocuser(IPvPDroneManager droneManager, IPvPDroneFocusSoundPicker soundPicker /* , IPvPPrioritisedSoundPlayer soundPlayer */)
        {
            PvPHelper.AssertIsNotNull(droneManager, soundPicker /*, soundPlayer */);

            _droneManager = droneManager;
            _soundPicker = soundPicker;
            // _soundPlayer = soundPlayer;
        }

        public void ToggleDroneConsumerFocus(IPvPDroneConsumer droneConsumer, bool isTriggeredByPlayer)
        {
            Assert.IsNotNull(droneConsumer);

            PvPDroneConsumerState preFocusState = droneConsumer.State;
            _droneManager.ToggleDroneConsumerFocus(droneConsumer);
            PvPDroneConsumerState postFocusState = droneConsumer.State;

            if (isTriggeredByPlayer)
            {
                try
                {
                    PvPPrioritisedSoundKey sound = _soundPicker.PickSound(preFocusState, postFocusState);
                    //Debug.Log(sound.Key);
                    // _soundPlayer.PlaySound(sound);

                    if (droneConsumer.NumOfDronesRequired == PvPRepairManager.NUM_OF_DRONES_REQUIRED_FOR_REPAIR)
                    {
                        PlayerTriggeredRepair?.Invoke(this, EventArgs.Empty);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }

            }
        }
    }
}