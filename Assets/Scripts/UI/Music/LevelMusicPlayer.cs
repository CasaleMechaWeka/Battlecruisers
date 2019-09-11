using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Threading;
using System;

namespace BattleCruisers.UI.Music
{
    /// <summary>
    /// Play danger music for 15 seconds after a danger event.  After this time
    /// return to the standard music.
    /// </summary>
    /// FELIX  Update tests
    /// FELIX  Rename to LevelMusicPlayer?
    public class LevelMusicPlayer
    {
        private readonly ILayeredMusicPlayer _musicPlayer;
        private readonly IDangerMonitor _dangerMonitor;
        private readonly IDeferrer _deferrer;
        private int _currentDangerCount;

        private const float DANGER_MUSIC_PLAY_TIME_IN_S = 15;

        public LevelMusicPlayer(
            ILayeredMusicPlayer musicPlayer,
            IDangerMonitor dangerMonitor,
            IDeferrer deferrer,
            IBattleCompletionHandler battleCompletionHandler)
        {
            Helper.AssertIsNotNull(musicPlayer, dangerMonitor, deferrer, battleCompletionHandler);

            _musicPlayer = musicPlayer;
            _dangerMonitor = dangerMonitor;
            _deferrer = deferrer;
            _currentDangerCount = 0;

            _dangerMonitor.Danger += _dangerMonitor_Danger;
            battleCompletionHandler.BattleCompleted += (sender, e) => _musicPlayer.Stop();

            _musicPlayer.Play();
        }

        private void _dangerMonitor_Danger(object sender, EventArgs e)
        {
            _currentDangerCount++;
            int cachedDangerCount = _currentDangerCount;

            if (_currentDangerCount == 1)
            {
                _musicPlayer.PlaySecondary();
            }

            _deferrer.Defer(() => OnDangerComplete(cachedDangerCount), DANGER_MUSIC_PLAY_TIME_IN_S);
        }

        private void OnDangerComplete(int dangerCount)
        {
            if (dangerCount == _currentDangerCount)
            {
                _musicPlayer.StopSecondary();
            }
        }
    }
}