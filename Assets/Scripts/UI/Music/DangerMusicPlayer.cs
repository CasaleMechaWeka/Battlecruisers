using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using System;

namespace BattleCruisers.UI.Music
{
    /// <summary>
    /// Play danger music for 15 seconds after a danger event.  After this time
    /// return to the standard music.
    /// </summary>
    public class DangerMusicPlayer
    {
        private readonly IMusicPlayer _musicPlayer;
        private readonly IDangerMonitor _dangerMonitor;
        private readonly IDeferrer _deferrer;
        private int _currentDangerCount;

        private const float DANGER_MUSIC_PLAY_TIME_IN_S = 15;

        public DangerMusicPlayer(
            IMusicPlayer musicPlayer,
            IDangerMonitor dangerMonitor,
            IDeferrer deferrer)
        {
            Helper.AssertIsNotNull(musicPlayer, dangerMonitor, deferrer);

            _musicPlayer = musicPlayer;
            _dangerMonitor = dangerMonitor;
            _deferrer = deferrer;
            _currentDangerCount = 0;

            _dangerMonitor.Danger += _dangerMonitor_Danger;
        }

        private void _dangerMonitor_Danger(object sender, EventArgs e)
        {
            _currentDangerCount++;
            int cachedDangerCount = _currentDangerCount;
            _musicPlayer.PlayDangerMusic();
            _deferrer.Defer(() => OnDangerComplete(cachedDangerCount), DANGER_MUSIC_PLAY_TIME_IN_S);
        }

        private void OnDangerComplete(int dangerCount)
        {
            if (dangerCount == _currentDangerCount)
            {
                _musicPlayer.PlayBattleSceneMusic();
            }
        }
    }
}