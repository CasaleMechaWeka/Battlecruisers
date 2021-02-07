namespace BattleCruisers.UI.Sound.Players
{
    public class DummySoundPlayer : IPrioritisedSoundPlayer
    {
        public bool Enabled { get; set; }

        public void PlaySound(PrioritisedSoundKey soundKey)
        {
            // empty
        }
    }
}
