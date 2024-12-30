using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.AudioSources
{
    public class PvPAudioSourceGroup : IPvPManagedDisposable
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IList<IPvPAudioSource> _audioSources;

        public PvPAudioSourceGroup(ISettingsManager settingsManager, params IPvPAudioSource[] audioSources)
            : this(settingsManager, audioSources.ToList())
        {
        }

        public PvPAudioSourceGroup(ISettingsManager settingsManager, IList<IPvPAudioSource> audioSources)
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
            foreach (IPvPAudioSource audioSource in _audioSources)
            {
                audioSource.Volume = _settingsManager.EffectVolume * _settingsManager.MasterVolume;
            }
        }

        public void DisposeManagedState()
        {
            _settingsManager.SettingsSaved -= _settingsManager_SettingsSaved;
        }
    }
}