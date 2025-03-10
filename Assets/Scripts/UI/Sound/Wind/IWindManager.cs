using BattleCruisers.Utils;

namespace BattleCruisers.UI.Sound.Wind
{
    public interface IWindManager : IManagedDisposable
    {
        void Play();
        void Stop();
    }
}