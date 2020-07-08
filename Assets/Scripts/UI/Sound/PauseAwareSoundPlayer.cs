using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;
using System.Threading.Tasks;

namespace BattleCruisers.UI.Sound
{
    /// <summary>
    /// Stop current sound when the game is paused.
    /// 
    /// Do not play sounds if the game is paused.
    /// </summary>
    /// FELIX  Test :)
    public class PauseAwareSoundPlayer : ISingleSoundPlayer
    {
        private readonly ISingleSoundPlayer _corePlayer;
        private readonly IPauseGameManager _pauseGameManager;

        public bool IsPlayingSound => _corePlayer.IsPlayingSound;

        public PauseAwareSoundPlayer(ISingleSoundPlayer corePlayer, IPauseGameManager pauseGameManager)
        {
            Helper.AssertIsNotNull(corePlayer, pauseGameManager);

            _corePlayer = corePlayer;
            _pauseGameManager = pauseGameManager;

            pauseGameManager.GamePaused += PauseGameManager_GamePaused;
        }

        private void PauseGameManager_GamePaused(object sender, EventArgs e)
        {
            _corePlayer.Stop();
        }

        public async Task PlaySoundAsync(ISoundKey soundKey, bool loop = false)
        {
            if (!_pauseGameManager.IsGamePaused)
            {
                await _corePlayer.PlaySoundAsync(soundKey, loop);
            }
        }

        public void PlaySound(IAudioClipWrapper sound, bool loop = false)
        {
            if (!_pauseGameManager.IsGamePaused)
            {
                _corePlayer.PlaySound(sound, loop);
            }
        }

        public void Stop()
        {
            _corePlayer.Stop();
        }
    }
}
