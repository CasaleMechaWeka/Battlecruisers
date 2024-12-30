using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Audio;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music
{
    public class PvPLayeredMusicPlayer : IPvPLayeredMusicPlayer
    {
        private readonly IPvPAudioVolumeFade _audioVolumeFade;
        private readonly IPvPAudioSource _primarySource, _secondarySource;
        private readonly ISettingsManager _settingsManager;
        private bool _isDisposed, _isPlayingSecondary;

        public const float FADE_TIME_IN_S = 2;

        public PvPLayeredMusicPlayer(
            IPvPAudioVolumeFade audioVolumeFade,
            IPvPAudioSource primarySource,
            IPvPAudioSource secondarySource,
            ISettingsManager settingsManager)
        {
            PvPHelper.AssertIsNotNull(audioVolumeFade, primarySource, secondarySource, settingsManager);

            _audioVolumeFade = audioVolumeFade;
            _primarySource = primarySource;
            _secondarySource = secondarySource;
            _settingsManager = settingsManager;
            _isDisposed = false;
            _isPlayingSecondary = false;

            _settingsManager.SettingsSaved += _settingsManager_SettingsSaved;
        }

        private void _settingsManager_SettingsSaved(object sender, EventArgs e)
        {
            _primarySource.Volume = _settingsManager.MusicVolume * _settingsManager.MasterVolume;
            _audioVolumeFade.Stop();
            _secondarySource.Volume = _isPlayingSecondary ? _settingsManager.MusicVolume * _settingsManager.MasterVolume : 0;
        }

        public void Play()
        {
            Assert.IsFalse(_isDisposed);

            if (_primarySource.IsPlaying)
            {
                // Logging.Log(Tags.SOUND, $"Prmary source already playing, returning.");
                return;
            }

            _primarySource.Volume = _settingsManager.MusicVolume * _settingsManager.MasterVolume;
            _primarySource.Play(isSpatial: false, loop: true);

            _secondarySource.Volume = 0;
            _secondarySource.Play(isSpatial: false, loop: true);
        }

        public void PlaySecondary()
        {
            Assert.IsFalse(_isDisposed);
            _isPlayingSecondary = true;
            _audioVolumeFade.FadeToVolume(_secondarySource, targetVolume: _settingsManager.MusicVolume * _settingsManager.MasterVolume, FADE_TIME_IN_S);
        }

        public void StopSecondary()
        {
            Assert.IsFalse(_isDisposed);
            _isPlayingSecondary = false;
            _audioVolumeFade.FadeToVolume(_secondarySource, targetVolume: 0, FADE_TIME_IN_S);
        }

        public void Stop()
        {
            Assert.IsFalse(_isDisposed);

            if (!_primarySource.IsPlaying)
            {
                // Logging.Log(Tags.SOUND, $"No sound is playing, returning.");
                return;
            }

            _primarySource.Stop();
            _secondarySource.Stop();
        }

        public void DisposeManagedState()
        {
            if (!_isDisposed)
            {
                _primarySource.FreeAudioClip();
                _secondarySource.FreeAudioClip();
                _settingsManager.SettingsSaved -= _settingsManager_SettingsSaved;
                _isDisposed = true;
            }
        }
    }
}