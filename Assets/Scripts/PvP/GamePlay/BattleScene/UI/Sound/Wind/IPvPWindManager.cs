using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Wind
{
    public interface IPvPWindManager : IManagedDisposable
    {
        void Play();
        void Stop();
    }
}