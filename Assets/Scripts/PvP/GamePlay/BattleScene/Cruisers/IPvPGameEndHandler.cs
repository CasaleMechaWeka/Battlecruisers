using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    public interface IPvPGameEndHandler
    {
        void HandleCruiserDestroyed(bool wasPlayerVictory);
        void HandleGameEnd();
        void HandleCruiserDestroyed(bool wasPlayerVictory, long destructionScore);
        void RegisterAIOfLeftPlayer(IManagedDisposable ai_LeftPlayer);
        void RegisterAIOfRightPlayer(IManagedDisposable ai_RightPlayer);
    }
}