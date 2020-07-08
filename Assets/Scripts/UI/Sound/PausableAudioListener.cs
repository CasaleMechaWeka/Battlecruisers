using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound
{
    // FELIX  Test, use
    // FELIX  Fade sound in and out?
    public class PausableAudioListener
    {
        // FELIX  Abstract
        //private readonly AudioListener _audioListener;
        private readonly IPauseGameManager _pauseGameManager;

        public PausableAudioListener(IPauseGameManager pauseGameManager)
        //public PausableAudioListener(AudioListener audioListener, IPauseGameManager pauseGameManager)
        {
            Assert.IsNotNull(pauseGameManager);
            //Helper.AssertIsNotNull(audioListener, pauseGameManager);
            //_audioListener = audioListener;

            pauseGameManager.GamePaused += PauseGameManager_GamePaused; ;
            pauseGameManager.GameResumed += PauseGameManager_GameResumed;
        }

        private void PauseGameManager_GamePaused(object sender, EventArgs e)
        {
            AudioListener.pause = true;
        }

        private void PauseGameManager_GameResumed(object sender, EventArgs e)
        {
            AudioListener.pause = false;
        }
    }
}