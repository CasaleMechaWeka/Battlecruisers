using BattleCruisers.Utils;

namespace BattleCruisers.UI.Music
{
    public interface ILayeredMusicPlayer : IManagedDisposable
    {
        void Play();
        void PlaySecondary();
        void StopSecondary();
        void Stop();
    }
}