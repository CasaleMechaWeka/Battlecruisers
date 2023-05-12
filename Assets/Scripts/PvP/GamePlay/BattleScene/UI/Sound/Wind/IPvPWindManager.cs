using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Wind
{
    public interface IPvPWindManager : IPvPManagedDisposable
    {
        void Play();
        void Stop();
    }
}