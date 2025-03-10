namespace BattleCruisers.UI.Sound.Players
{
    public interface IPrioritisedSoundPlayer
    {
        bool Enabled { get; set; }
        void PlaySound(PrioritisedSoundKey soundKey);
    }
}