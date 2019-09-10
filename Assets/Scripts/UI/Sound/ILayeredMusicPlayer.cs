namespace BattleCruisers.UI.Sound
{
    public interface ILayeredMusicPlayer
    {
        void Play();
        void PlaySecondary();
        void StopSecondary();
        void Stop();
    }
}