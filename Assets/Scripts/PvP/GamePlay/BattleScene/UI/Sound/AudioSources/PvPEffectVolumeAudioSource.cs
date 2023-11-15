using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.AudioSources
{
    public class PvPEffectVolumeAudioSource : PvPVolumeAwareAudioSource
    {
        private int _type = -1;
        public PvPEffectVolumeAudioSource(IPvPAudioSource audioSource, ISettingsManager settingsManager, int type)
            : base(audioSource, settingsManager)
        {
            _type = type;
        }

        public PvPEffectVolumeAudioSource(IPvPAudioSource audioSource, ISettingsManager settingsManager)
            : base(audioSource, settingsManager)
        {
            _type = -1;
        }

        protected override float GetVolume(ISettingsManager settingsManager)
        {

            if (settingsManager == null) // it means server
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