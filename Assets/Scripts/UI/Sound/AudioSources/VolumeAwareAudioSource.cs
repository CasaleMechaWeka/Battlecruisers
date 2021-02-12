using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;

namespace BattleCruisers.UI.Sound.AudioSources
{
    public abstract class VolumeAwareAudioSource : IManagedDisposable, IAudioSource
    {
        private readonly IAudioSource _audioSource;
        private readonly ISettingsManager _settingsManager;

        public bool IsPlaying => _audioSource.IsPlaying;
        public IAudioClipWrapper AudioClip { set => _audioSource.AudioClip = value; }
        public float Volume { get => _audioSource.Volume; set => _audioSource.Volume = value; }
        public Vector2 Position { get => _audioSource.Position; set => _audioSource.Position = value; }
        public bool IsActive { get => _audioSource.IsActive; set => _audioSource.IsActive = value; }

        protected VolumeAwareAudioSource(IAudioSource audioSource, ISettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(audioSource, settingsManager);

            _audioSource = audioSource;
            _settingsManager = settingsManager;

            SetVolume();

            _settingsManager.SettingsSaved += _settingsManager_SettingsSaved;
        }

        private void _settingsManager_SettingsSaved(object sender, System.EventArgs e)
        {
            SetVolume();
        }

        private void SetVolume()
        {
            _audioSource.Volume = GetVolume(_settingsManager);
        }

        protected abstract float GetVolume(ISettingsManager settingsManager);

        public void DisposeManagedState()
        {
            _settingsManager.SettingsSaved -= _settingsManager_SettingsSaved;
        }

        public void Play(bool isSpatial = true, bool loop = false)
        {
            _audioSource.Play(isSpatial, loop);
        }

        public void Stop()
        {
            _audioSource.Stop();
        }

        public void FreeAudioClip()
        {
            _audioSource.FreeAudioClip();
        }
    }
}