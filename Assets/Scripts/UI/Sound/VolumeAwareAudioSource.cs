using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;

namespace BattleCruisers.UI.Sound
{
    // FELIX  Use everywhere!  Can undo a lot of the changes I've been making? :P
    public class VolumeAwareAudioSource : IManagedDisposable
    {
        private readonly IAudioSource _audioSource;
        private readonly ISettingsManager _settingsManager;

        public VolumeAwareAudioSource(IAudioSource audioSource, ISettingsManager settingsManager)
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
            _audioSource.Volume = _settingsManager.EffectVolume;
        }

        public void DisposeManagedState()
        {
            _settingsManager.SettingsSaved -= _settingsManager_SettingsSaved;
        }
    }
}