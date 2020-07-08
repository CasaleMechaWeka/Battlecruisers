using System;

namespace BattleCruisers.Utils.BattleScene
{
    public interface IPauseGameManager
    {
        bool IsGamePaused { get; }

        event EventHandler GamePaused;
        event EventHandler GameResumed;

        void PauseGame();
        void ResumeGame();
    }
}