using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    public interface IPvPPauseGameManager
    {
        event EventHandler GamePaused;
        event EventHandler GameResumed;

        void PauseGame();
        void ResumeGame();
    }
}