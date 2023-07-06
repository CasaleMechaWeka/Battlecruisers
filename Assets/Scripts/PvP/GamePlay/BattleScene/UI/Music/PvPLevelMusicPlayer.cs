using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music
{
    public class PvPLevelMusicPlayer
    {
        private readonly IPvPLayeredMusicPlayer _musicPlayer;
        private readonly IPvPDangerMonitorSummariser _dangerMonitorSummariser;

        public PvPLevelMusicPlayer(
            IPvPLayeredMusicPlayer musicPlayer,
            IPvPDangerMonitorSummariser dangerMonitorSummariser,
            IPvPBattleCompletionHandler battleCompletionHandler)
        {
            PvPHelper.AssertIsNotNull(musicPlayer, dangerMonitorSummariser, battleCompletionHandler);

            _musicPlayer = musicPlayer;
            _dangerMonitorSummariser = dangerMonitorSummariser;

            _dangerMonitorSummariser.IsInDanger.ValueChanged += IsInDanger_ValueChanged;
            battleCompletionHandler.BattleCompleted += BattleCompletionHandler_BattleCompleted;

            _musicPlayer.Play();
        }

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