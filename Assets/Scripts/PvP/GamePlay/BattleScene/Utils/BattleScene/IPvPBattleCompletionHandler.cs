using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    public interface IPvPBattleCompletionHandler
    {
        event EventHandler BattleCompleted;

        void CompleteBattle(bool wasVictory, bool retryLevel);
        void CompleteBattle(bool wasVictory, bool retryLevel, long destructionScore);
    }
}