using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using System;

namespace BattleCruisers.Cruisers.Drones
{
    public class DroneFocusSoundPicker
    {
        //TODO update tests
        public PrioritisedSoundKey PickSound(DroneConsumerState preFocusState, DroneConsumerState postFocusState)
        {
            //Debug.Log($"{preFocusState}>{postFocusState}");

            return preFocusState switch
            {
                DroneConsumerState.Idle => postFocusState switch
                {
                    DroneConsumerState.Active or DroneConsumerState.Focused => PrioritisedSoundKeys.Events.Drones.Focusing,
                    DroneConsumerState.AllFocused => PrioritisedSoundKeys.Events.Drones.AllFocused,
                    _ => throw new ArgumentException(),
                },
                DroneConsumerState.Active => postFocusState switch
                {
                    DroneConsumerState.Idle => PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToFocus,
                    DroneConsumerState.Focused => PrioritisedSoundKeys.Events.Drones.Focusing,
                    DroneConsumerState.AllFocused => PrioritisedSoundKeys.Events.Drones.AllFocused,
                    _ => throw new ArgumentException(),
                },
                DroneConsumerState.Focused => postFocusState switch
                {
                    DroneConsumerState.Idle => PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToFocus,
                    DroneConsumerState.Active => PrioritisedSoundKeys.Events.Drones.Dispersing,
                    DroneConsumerState.AllFocused => PrioritisedSoundKeys.Events.Drones.AllFocused,
                    _ => throw new ArgumentException(),
                },
                DroneConsumerState.AllFocused => postFocusState switch
                {
                    DroneConsumerState.Idle => PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToFocus,
                    DroneConsumerState.Active or DroneConsumerState.Focused => PrioritisedSoundKeys.Events.Drones.Dispersing,
                    _ => PrioritisedSoundKeys.Events.Drones.AllFocused,
                },
                _ => throw new ArgumentException(),
            };
        }
    }
}