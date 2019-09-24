namespace BattleCruisers.UI.Sound
{
    public interface IPrioritisedSoundPlayer
    {
        bool Enabled { get; set; }
        void PlaySound(PrioritisedSoundKey soundKey);
    }
}