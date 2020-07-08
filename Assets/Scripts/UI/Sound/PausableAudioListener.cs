using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;

namespace BattleCruisers.UI.Sound
{
    public class PausableAudioListener
    {
        private readonly IAudioListener _audioListener;
        private readonly IPauseGameManager _pauseGameManager;

        public PausableAudioListener(IAudioListener audioListener, IPauseGameManager pauseGameManager)
        {
            Helper.AssertIsNotNull(audioListener, pauseGameManager);
            
            _audioListener = audioListener;

            pauseGameManager.GamePaused += PauseGameManager_GamePaused; ;
            pauseGameManager.GameResumed += PauseGameManager_GameResumed;
        }

        private void PauseGameManager_GamePaused(object sender, EventArgs e)
        {
            _audioListener.Pause();
        }

        private void PauseGameManager_GameResumed(object sender, EventArgs e)
        {
            _audioListener.Resume();
        }
    }
}