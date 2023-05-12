using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music
{
    public interface IPvPLayeredMusicPlayer : IPvPManagedDisposable
    {
        void Play();
        void PlaySecondary();
        void StopSecondary();
        void Stop();
    }
}