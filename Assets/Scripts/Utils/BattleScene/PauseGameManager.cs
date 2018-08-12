using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene
{
    // FELIX  Test :)
    public class PauseGameManager : IPauseGameManager
    {
        private readonly ITime _time;

        public event EventHandler GamePaused;
        public event EventHandler GameResumed;

        public PauseGameManager(ITime time)
        {
            Assert.IsNotNull(time);
            _time = time;
        }

        public void PauseGame()
        {
            _time.TimeScale = 0;

            if (GamePaused != null)
            {
                GamePaused.Invoke(this, EventArgs.Empty);
            }
        }

        public void ResumeGame()
        {
            _time.TimeScale = 1;

            if (GameResumed != null)
            {
                GameResumed.Invoke(this, EventArgs.Empty);
            }
        }
    }
}