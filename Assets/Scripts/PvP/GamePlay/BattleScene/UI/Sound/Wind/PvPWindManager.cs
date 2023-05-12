using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Wind
{
    public class PvPWindManager : IPvPWindManager
    {
        private readonly IPvPAudioSource _audioSource;
        private readonly IPvPCamera _camera;
        private readonly IPvPVolumeCalculator _volumeCalculator;
        private readonly ISettingsManager _settingsManager;

        public PvPWindManager(
            IPvPAudioSource audioSource,
            IPvPCamera camera,
            IPvPVolumeCalculator volumeCalculator,
            ISettingsManager settingsManager)
        {
            PvPHelper.AssertIsNotNull(audioSource, camera, volumeCalculator, settingsManager);

            _audioSource = audioSource;
            _camera = camera;
            _volumeCalculator = volumeCalculator;
            _settingsManager = settingsManager;

            _camera.OrthographicSizeChanged += _camera_OrthographicSizeChanged;
            _settingsManager.SettingsSaved += _settingsManager_SettingsSaved;

            FindVolume();
        }

        private void _camera_OrthographicSizeChanged(object sender, EventArgs e)
        {
            FindVolume();
        }

        private void _settingsManager_SettingsSaved(object sender, EventArgs e)
        {
            FindVolume();
        }

        private void FindVolume()
        {
            _audioSource.Volume = _volumeCalculator.FindVolume(_camera.OrthographicSize);
        }

        public void Play()
        {
            _audioSource.Play(isSpatial: false, loop: true);
        }

        public void Stop()
        {
            _audioSource.Stop();
        }

        public void DisposeManagedState()
        {
            _camera.OrthographicSizeChanged -= _camera_OrthographicSizeChanged;
            _settingsManager.SettingsSaved -= _settingsManager_SettingsSaved;
        }
    }
}