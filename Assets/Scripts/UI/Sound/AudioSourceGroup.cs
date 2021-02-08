using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound
{
    public class AudioSourceGroup : IManagedDisposable
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IAudioSource[] _audioSources;

        public AudioSourceGroup(ISettingsManager settingsManager, params IAudioSource[] audioSources)
        {
            Helper.AssertIsNotNull(settingsManager, audioSources);
            Assert.IsTrue(audioSources.Length != 0);

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
                audioSource.Volume = _settingsManager.EffectVolume;
            }
        }

        public void DisposeManagedState()
        {
            _settingsManager.SettingsSaved -= _settingsManager_SettingsSaved;
        }
    }
}