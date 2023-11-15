using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones
{
    public interface IPvPDroneFocusSoundPicker
    {
        PvPPrioritisedSoundKey PickSound(PvPDroneConsumerState preFocusState, PvPDroneConsumerState postFocusState);
    }
}