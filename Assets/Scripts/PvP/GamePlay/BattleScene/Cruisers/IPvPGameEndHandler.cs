using BattleCruisers.AI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    public interface IPvPGameEndHandler
    {
        void HandleCruiserDestroyed(bool wasPlayerVictory);
        void HandleGameEnd();
        void HandleCruiserDestroyed(bool wasPlayerVictory, long destructionScore);
        void RegisterAIOfLeftPlayer(IArtificialIntelligence ai_LeftPlayer);
        void RegisterAIOfRightPlayer(IArtificialIntelligence ai_RightPlayer);
    }
}