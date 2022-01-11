using BattleCruisers.Data.Settings;
using BattleCruisers.Utils.PlatformAbstractions.Audio;

namespace BattleCruisers.UI.Sound.AudioSources
{
    public class EffectVolumeAudioSource : VolumeAwareAudioSource
    {
        public EffectVolumeAudioSource(IAudioSource audioSource, ISettingsManager settingsManager) 
            : base(audioSource, settingsManager)
        {
        }

        protected override float GetVolume(ISettingsManager settingsManager)
        {
            return settingsManager.EffectVolume*settingsManager.MasterVolume;
        }
    }
}