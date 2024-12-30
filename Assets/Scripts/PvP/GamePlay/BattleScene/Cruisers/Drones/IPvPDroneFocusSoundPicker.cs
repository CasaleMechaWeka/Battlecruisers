using BattleCruisers.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public interface IPvPDroneFocusSoundPicker
    {
        PrioritisedSoundKey PickSound(PvPDroneConsumerState preFocusState, PvPDroneConsumerState postFocusState);
    }
}