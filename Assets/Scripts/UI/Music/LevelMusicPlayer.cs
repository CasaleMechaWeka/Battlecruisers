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
            battleCompletionHandler.BattleCompleted += BattleCompletionHandler_BattleCompleted;

            _musicPlayer.Play();
        }

        // FELIX  Update tests :)
        private void BattleCompletionHandler_BattleCompleted(object sender, EventArgs e)
        {
            _musicPlayer.Stop();
            _musicPlayer.DisposeManagedState();
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