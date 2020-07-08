using System;

namespace BattleCruisers.Utils.BattleScene
{
    public interface IPauseGameManager
    {
        event EventHandler GamePaused;
        event EventHandler GameResumed;

        void PauseGame();
        void ResumeGame();
    }
}