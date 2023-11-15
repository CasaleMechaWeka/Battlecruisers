namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players
{
    public class PvPDummySoundPlayer : IPvPPrioritisedSoundPlayer
    {
        public bool Enabled { get; set; }

        public void PlaySound(PvPPrioritisedSoundKey soundKey)
        {
            // empty
        }
    }
}
