using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;

namespace BattleCruisers.UI.Sound.Wind
{
    public class WindManager
    {
        private readonly IAudioSource _audioSource;
        private readonly ICamera _camera;
        private readonly VolumeCalculator _volumeCalculator;
        private readonly SettingsManager _settingsManager;

        public WindManager(
            IAudioSource audioSource,
            ICamera camera,
            VolumeCalculator volumeCalculator,
            SettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(audioSource, camera, volumeCalculator, settingsManager);

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