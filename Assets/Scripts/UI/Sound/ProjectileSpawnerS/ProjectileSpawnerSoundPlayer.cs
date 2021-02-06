using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public abstract class ProjectileSpawnerSoundPlayer : IProjectileSpawnerSoundPlayer
    {
        protected readonly IAudioSource _audioSource;
        private readonly ISettingsManager _settingsManager;

        protected ProjectileSpawnerSoundPlayer(IAudioClipWrapper audioClip, IAudioSource audioSource, ISettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(audioClip, audioSource, settingsManager);

            _settingsManager = settingsManager;
            _audioSource = audioSource;
            _audioSource.AudioClip = audioClip;
            _audioSource.Volume = _settingsManager.EffectVolume;

            _settingsManager.SettingsSaved += _settingsManager_SettingsSaved;
        }

        // FELIX  Add test :)
        private void _settingsManager_SettingsSaved(object sender, EventArgs e)
        {
            _audioSource.Volume = _settingsManager.EffectVolume;
        }

        public abstract void OnProjectileFired();
    }
}
