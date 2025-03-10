using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.UI.Sound.AudioSources
{
    public class AudioSourceGroup : IManagedDisposable
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IList<IAudioSource> _audioSources;

        public AudioSourceGroup(ISettingsManager settingsManager, params IAudioSource[] audioSources)
            : this(settingsManager, audioSources.ToList())
        {
        }

        public AudioSourceGroup(ISettingsManager settingsManager, IList<IAudioSource> audioSources)
        {
            Helper.AssertIsNotNull(settingsManager, audioSources);

            _settingsManager = settingsManager;
            _audioSources = audioSources;

            SetVolume();

            _settingsManager.SettingsSaved += _settingsManager_SettingsSaved;
        }

        private void _settingsManager_SettingsSaved(object sender, System.EventArgs e)
        {
            SetVolume();
        }

        private void SetVolume()
        {
            foreach (IAudioSource audioSource in _audioSources)
            {
                audioSource.Volume = _settingsManager.EffectVolume*_settingsManager.MasterVolume;
            }
        }

        public void DisposeManagedState()
        {
            _settingsManager.SettingsSaved -= _settingsManager_SettingsSaved;
        }
    }
}