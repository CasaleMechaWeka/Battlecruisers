using System;

namespace BattleCruisers.Utils.BattleScene
{
    public interface IBattleCompletionHandler
    {
        event EventHandler BattleCompleted;

        void CompleteBattle(bool wasVictory, bool retryLevel);
        void CompleteBattle(bool wasVictory, bool retryLevel, long destructionScore);
    }
}