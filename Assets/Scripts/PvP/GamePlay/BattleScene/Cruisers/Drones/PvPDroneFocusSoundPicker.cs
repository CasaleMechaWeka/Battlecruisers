using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using System;
namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public class PvPDroneFocusSoundPicker : IPvPDroneFocusSoundPicker
    {
        //TODO update tests
        public PrioritisedSoundKey PickSound(PvPDroneConsumerState preFocusState, PvPDroneConsumerState postFocusState)
        {
            //Debug.Log($"{preFocusState}>{postFocusState}");

            switch (preFocusState)
            {
                case PvPDroneConsumerState.Idle:
                    switch (postFocusState)
                    {
                        case PvPDroneConsumerState.Active:
                        case PvPDroneConsumerState.Focused:
                            return PrioritisedSoundKeys.Events.Drones.Focusing;
                        case PvPDroneConsumerState.AllFocused:
                            return PrioritisedSoundKeys.Events.Drones.AllFocused;
                        default:
                            throw new ArgumentException();
                    }

                case PvPDroneConsumerState.Active:
                    switch (postFocusState)
                    {
                        case PvPDroneConsumerState.Idle:
                            return PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToFocus;
                        case PvPDroneConsumerState.Focused:
                            return PrioritisedSoundKeys.Events.Drones.Focusing;
                        case PvPDroneConsumerState.AllFocused:
                            return PrioritisedSoundKeys.Events.Drones.AllFocused;
                        default:
                            throw new ArgumentException();
                    }

                case PvPDroneConsumerState.Focused:
                    switch (postFocusState)
                    {
                        case PvPDroneConsumerState.Idle:
                            return PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToFocus;
                        case PvPDroneConsumerState.Active:
                            return PrioritisedSoundKeys.Events.Drones.Dispersing;
                        case PvPDroneConsumerState.AllFocused:
                            return PrioritisedSoundKeys.Events.Drones.AllFocused;
                        default:
                            throw new ArgumentException();
                    }

                case PvPDroneConsumerState.AllFocused:
                    switch (postFocusState)
                    {
                        case PvPDroneConsumerState.Idle:
                            return PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToFocus;
                        case PvPDroneConsumerState.Active:
                        case PvPDroneConsumerState.Focused:
                            return PrioritisedSoundKeys.Events.Drones.Dispersing;
                        default:
                            return PrioritisedSoundKeys.Events.Drones.AllFocused;
                    }
                default:
                    throw new ArgumentException();
            }

        }
    }
}