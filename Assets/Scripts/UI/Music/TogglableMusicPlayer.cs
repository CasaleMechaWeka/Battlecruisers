using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Music
{
    public class TogglableMusicPlayer : IMusicPlayer
    {
        private readonly IMusicPlayer _corePlayer;
        private readonly ISettingsManager _settings;

        public TogglableMusicPlayer(IMusicPlayer corePlayer, ISettingsManager settings)
        {
            Helper.AssertIsNotNull(corePlayer, settings);

            _corePlayer = corePlayer;
            _settings = settings;
        }

        public void PlayScreensSceneMusic()
        {
            if (!_settings.MuteMusic)
            {
                _corePlayer.PlayScreensSceneMusic();
            }
        }

        public void PlayVictoryMusic()
        {
            if (!_settings.MuteMusic)
            {
                _corePlayer.PlayVictoryMusic();
            }
        }

        public void PlayDefeatMusic()
        {
            if (!_settings.MuteMusic)
            {
                _corePlayer.PlayDefeatMusic();
            }
        }

        public void PlayLoadingMusic()
        {
            if (!_settings.MuteMusic)
            {
                _corePlayer.PlayLoadingMusic();
            }
        }

        public void Stop()
        {
            _corePlayer.Stop();
        }
    }
}