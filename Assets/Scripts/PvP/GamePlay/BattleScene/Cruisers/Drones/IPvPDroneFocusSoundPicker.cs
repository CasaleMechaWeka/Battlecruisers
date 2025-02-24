using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public interface IPvPDroneFocusSoundPicker
    {
        PrioritisedSoundKey PickSound(DroneConsumerState preFocusState, DroneConsumerState postFocusState);
    }
}