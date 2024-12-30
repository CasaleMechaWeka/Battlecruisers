using BattleCruisers.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players
{
    public interface IPvPPrioritisedSoundPlayer
    {
        bool Enabled { get; set; }
        void PlaySound(PrioritisedSoundKey soundKey);
    }
}