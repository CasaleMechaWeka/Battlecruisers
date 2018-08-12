using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene
{
    public class PauseGameManager : IPauseGameManager
    {
        private readonly ITime _time;

        private bool IsGamePaused
        {
            get
            {
                return Mathf.Approximately(_time.TimeScale, 0);
            }
        }

        public event EventHandler GamePaused;
        public event EventHandler GameResumed;

        public PauseGameManager(ITime time)
        {
            Assert.IsNotNull(time);
            _time = time;
        }

        public void PauseGame()
        {
            if (IsGamePaused)
            {
                return;
            }

            _time.TimeScale = 0;

            if (GamePaused != null)
            {
                GamePaused.Invoke(this, EventArgs.Empty);
            }
        }

        public void ResumeGame()
        {
            if (!IsGamePaused)
            {
                return;
            }

            _time.TimeScale = 1;

            if (GameResumed != null)
            {
                GameResumed.Invoke(this, EventArgs.Empty);
            }
        }
    }
}