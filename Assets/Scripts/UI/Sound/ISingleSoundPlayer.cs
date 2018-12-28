namespace BattleCruisers.UI.Sound
{
    public interface ISingleSoundPlayer
    {
        bool IsPlayingSound { get; }

        void PlaySound(ISoundKey soundKey, bool loop = false);
        void Stop();
    }
}
