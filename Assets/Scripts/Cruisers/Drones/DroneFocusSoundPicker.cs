using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using System;

namespace BattleCruisers.Cruisers.Drones
{
    public class DroneFocusSoundPicker : IDroneFocusSoundPicker
    {
        public PrioritisedSoundKey PickSound(DroneConsumerState preFocusState, DroneConsumerState postFocusState)
        {
            switch (postFocusState)
            {
                case DroneConsumerState.Idle:
                    return PrioritisedSoundKeys.Events.Drones.NotEnoughDronesToFocus;

                case DroneConsumerState.Active:
                    switch (preFocusState)
                    {
                        case DroneConsumerState.Idle:
                            return PrioritisedSoundKeys.Events.Drones.Focusing;

                        case DroneConsumerState.Active:
                            return PrioritisedSoundKeys.Events.Drones.AllFocused;

                        case DroneConsumerState.Focused:
                            return PrioritisedSoundKeys.Events.Drones.Dispersing;

                        default:
                            throw new ArgumentException();
                    }

                case DroneConsumerState.Focused:
                    return PrioritisedSoundKeys.Events.Drones.AllFocused;

                default:
                    throw new ArgumentException();
            }
        }
    }
}