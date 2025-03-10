using BattleCruisers.UI.Sound;

namespace BattleCruisers.Cruisers.Drones
{
    public interface IDroneFocusSoundPicker
    {
        PrioritisedSoundKey PickSound(DroneConsumerState preFocusState, DroneConsumerState postFocusState);
    }
}