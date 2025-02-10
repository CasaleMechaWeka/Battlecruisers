using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music
{
    public interface IPvPLayeredMusicPlayer : IManagedDisposable
    {
        void Play();
        void PlaySecondary();
        void StopSecondary();
        void Stop();
    }
}