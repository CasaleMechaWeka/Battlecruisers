using System;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene
{
    public class PauseGameManager : IPauseGameManager
    {
        private readonly ITime _time;
        private float _prePauseTimeScale;

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
            _time.TimeScale = 1;
        }

        public void PauseGame()
        {
            if (IsGamePaused)
            {
                return;
            }

            _prePauseTimeScale = _time.TimeScale;
            _time.TimeScale = 0;

            GamePaused?.Invoke(this, EventArgs.Empty);
        }

        public void ResumeGame()
        {
            if (!IsGamePaused)
            {
                return;
            }

            _time.TimeScale = _prePauseTimeScale;

            GameResumed?.Invoke(this, EventArgs.Empty);
        }
    }
}