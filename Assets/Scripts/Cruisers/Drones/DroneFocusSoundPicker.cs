using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using System;
using UnityEngine;
namespace BattleCruisers.Cruisers.Drones
{
    public class DroneFocusSoundPicker : IDroneFocusSoundPicker
    {
        //TODO update tests
        public PrioritisedSoundKey PickSound(DroneConsumerState preFocusState, DroneConsumerState postFocusState)
        {
            //Debug.Log($"{preFocusState}>{postFocusState}");
            
            switch (preFocusState)
            {
                case DroneConsumerState.Idle:
                    switch(postFocusState)
                    {
                        case DroneConsumerState.Active:
                        case DroneConsumerState.Focused:
                            return PrioritisedSoundKeys.Events.Drones.Focusing;
                        case DroneConsumerState.AllFocused:
                            return PrioritisedSoundKeys.Events.Drones.AllFocused;
                        default:
                            throw new ArgumentException();
                    }

                case DroneConsumerState.Active:
                    switch(postFocusState)
                    {
                        case DroneConsumerState.Idle:
                            return PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToFocus;
                        case DroneConsumerState.Focused:
                            return PrioritisedSoundKeys.Events.Drones.Focusing;
                        case DroneConsumerState.AllFocused:
                            return PrioritisedSoundKeys.Events.Drones.AllFocused;
                        default:
                            throw new ArgumentException();
                    }

                case DroneConsumerState.Focused:
                    switch(postFocusState)
                    {
                        case DroneConsumerState.Idle:
                            return PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToFocus;
                        case DroneConsumerState.Active:
                            return PrioritisedSoundKeys.Events.Drones.Dispersing;
                        case DroneConsumerState.AllFocused:
                            return PrioritisedSoundKeys.Events.Drones.AllFocused;
                        default:
                            throw new ArgumentException();
                    }

                case DroneConsumerState.AllFocused:
                    switch(postFocusState)
                    {
                        case DroneConsumerState.Idle:
                            return PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToFocus;
                        case DroneConsumerState.Active:
                        case DroneConsumerState.Focused:
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