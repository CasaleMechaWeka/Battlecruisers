using BattleCruisers.Data.Settings;
using BattleCruisers.Utils.PlatformAbstractions.Audio;

namespace BattleCruisers.UI.Sound.AudioSources
{
    public class MusicVolumeAudioSource : VolumeAwareAudioSource
    {
        public MusicVolumeAudioSource(IAudioSource audioSource, ISettingsManager settingsManager) 
            : base(audioSource, settingsManager)
        {
        }

        protected override float GetVolume(ISettingsManager settingsManager)
        {
            return settingsManager.MusicVolume;
        }
    }
}