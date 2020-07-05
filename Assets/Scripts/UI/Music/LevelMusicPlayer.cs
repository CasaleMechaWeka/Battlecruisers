using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using System;

namespace BattleCruisers.UI.Music
{
    public class LevelMusicPlayer
    {
        private readonly ILayeredMusicPlayer _musicPlayer;
        private readonly IDangerMonitorSummariser _dangerMonitorSummariser;

        public LevelMusicPlayer(
            ILayeredMusicPlayer musicPlayer,
            IDangerMonitorSummariser dangerMonitorSummariser,
            IBattleCompletionHandler battleCompletionHandler)
        {
            Helper.AssertIsNotNull(musicPlayer, dangerMonitorSummariser, battleCompletionHandler);

            _musicPlayer = musicPlayer;
            _dangerMonitorSummariser = dangerMonitorSummariser;

            _dangerMonitorSummariser.IsInDanger.ValueChanged += IsInDanger_ValueChanged;
            battleCompletionHandler.BattleCompleted += (sender, e) => _musicPlayer.Stop();

            _musicPlayer.Play();
        }

        private void IsInDanger_ValueChanged(object sender, EventArgs e)
        {
            if (_dangerMonitorSummariser.IsInDanger.Value)
            {
                _musicPlayer.PlaySecondary();
            }
            else
            {
                _musicPlayer.StopSecondary();
            }
        }
    }
}