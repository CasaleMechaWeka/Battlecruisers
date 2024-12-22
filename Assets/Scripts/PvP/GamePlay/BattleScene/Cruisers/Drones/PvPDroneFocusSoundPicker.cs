using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using System;
using UnityEngine;
namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public class PvPDroneFocusSoundPicker : IPvPDroneFocusSoundPicker
    {
        //TODO update tests
        public PvPPrioritisedSoundKey PickSound(PvPDroneConsumerState preFocusState, PvPDroneConsumerState postFocusState)
        {
            //Debug.Log($"{preFocusState}>{postFocusState}");

            switch (preFocusState)
            {
                case PvPDroneConsumerState.Idle:
                    switch (postFocusState)
                    {
                        case PvPDroneConsumerState.Active:
                        case PvPDroneConsumerState.Focused:
                            return PvPPrioritisedSoundKeys.PvPEvents.PvPDrones.Focusing;
                        case PvPDroneConsumerState.AllFocused:
                            return PvPPrioritisedSoundKeys.PvPEvents.PvPDrones.AllFocused;
                        default:
                            throw new ArgumentException();
                    }

                case PvPDroneConsumerState.Active:
                    switch (postFocusState)
                    {
                        case PvPDroneConsumerState.Idle:
                            return PvPPrioritisedSoundKeys.PvPEvents.PvPDrones.NotEnoughDronesToFocus;
                        case PvPDroneConsumerState.Focused:
                            return PvPPrioritisedSoundKeys.PvPEvents.PvPDrones.Focusing;
                        case PvPDroneConsumerState.AllFocused:
                            return PvPPrioritisedSoundKeys.PvPEvents.PvPDrones.AllFocused;
                        default:
                            throw new ArgumentException();
                    }

                case PvPDroneConsumerState.Focused:
                    switch (postFocusState)
                    {
                        case PvPDroneConsumerState.Idle:
                            return PvPPrioritisedSoundKeys.PvPEvents.PvPDrones.NotEnoughDronesToFocus;
                        case PvPDroneConsumerState.Active:
                            return PvPPrioritisedSoundKeys.PvPEvents.PvPDrones.Dispersing;
                        case PvPDroneConsumerState.AllFocused:
                            return PvPPrioritisedSoundKeys.PvPEvents.PvPDrones.AllFocused;
                        default:
                            throw new ArgumentException();
                    }

                case PvPDroneConsumerState.AllFocused:
                    switch (postFocusState)
                    {
                        case PvPDroneConsumerState.Idle:
                            return PvPPrioritisedSoundKeys.PvPEvents.PvPDrones.NotEnoughDronesToFocus;
                        case PvPDroneConsumerState.Active:
                        case PvPDroneConsumerState.Focused:
                            return PvPPrioritisedSoundKeys.PvPEvents.PvPDrones.Dispersing;
                        default:
                            return PvPPrioritisedSoundKeys.PvPEvents.PvPDrones.AllFocused;
                    }
                default:
                    throw new ArgumentException();
            }

        }
    }
}