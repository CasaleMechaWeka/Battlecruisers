
using BattleCruisers.Data.Settings;
using BattleCruisers.Utils.PlatformAbstractions.Audio;

namespace BattleCruisers.UI.Sound.AudioSources
{
    public class EffectVolumeAudioSource : VolumeAwareAudioSource
    {
        private int _type = -1;
        public EffectVolumeAudioSource(IAudioSource audioSource, int type)
            : base(audioSource)
        {
            _type = type;
        }

        public EffectVolumeAudioSource(IAudioSource audioSource)
            : base(audioSource)
        {
            _type = -1;
        }

        protected override float GetVolume(SettingsManager settingsManager)
        {
            if (settingsManager == null)
                return 0f;
            if (_type == 0)
            {
                return settingsManager.AlertVolume * settingsManager.MasterVolume;
            }
            else if (_type == 1)
            {
                return settingsManager.InterfaceVolume * settingsManager.MasterVolume;
            }
            else if (_type == 2)
            {
                return settingsManager.AmbientVolume * settingsManager.MasterVolume;
            }
            else if (_type == 3)
            {
                return settingsManager.MusicVolume * settingsManager.MasterVolume;
            }
            else
            {
                return settingsManager.EffectVolume * settingsManager.MasterVolume;
            }

        }
    }
}