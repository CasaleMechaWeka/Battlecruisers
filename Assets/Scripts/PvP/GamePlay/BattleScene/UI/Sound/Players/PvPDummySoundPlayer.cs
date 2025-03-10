using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Players;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players
{
    public class PvPDummySoundPlayer : IPrioritisedSoundPlayer
    {
        public bool Enabled { get; set; }

        public void PlaySound(PrioritisedSoundKey soundKey)
        {
            // empty
        }
    }
}
