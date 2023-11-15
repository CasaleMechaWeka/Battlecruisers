using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    public interface IPvPGameEndHandler
    {
        void HandleCruiserDestroyed(bool wasPlayerVictory);
        void HandleGameEnd();
        void HandleCruiserDestroyed(bool wasPlayerVictory, long destructionScore);
        void RegisterAIOfLeftPlayer(IPvPArtificialIntelligence ai_LeftPlayer);
        void RegisterAIOfRightPlayer(IPvPArtificialIntelligence ai_RightPlayer);
    }
}