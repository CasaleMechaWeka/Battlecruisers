using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using UnityCommon.Properties;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    /// <summary>
    /// When drones go from idle to active, play a drones sound.
    /// </summary>
    /// FELIX  Update tests
    public class DroneSoundFeedback : IManagedDisposable
    {
        private readonly IBroadcastingProperty<bool> _parentCruiserHasActiveDrones;
        private readonly IAudioSource _audioSource;
        private readonly ISettingsManager _settingsManager;

        public DroneSoundFeedback(
            IBroadcastingProperty<bool> parentCruiserHasActiveDrones, 
            IAudioSource audioSource,
            ISettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(parentCruiserHasActiveDrones, audioSource, settingsManager);

            _parentCruiserHasActiveDrones = parentCruiserHasActiveDrones;
            _audioSource = audioSource;
            _settingsManager = settingsManager;
            
            SetVolume();

            _parentCruiserHasActiveDrones.ValueChanged += _parentCruiserHasActiveDrones_ValueChanged;
            _settingsManager.SettingsSaved += _settingsManager_SettingsSaved;
        }

        private void _parentCruiserHasActiveDrones_ValueChanged(object sender, EventArgs e)
        {
            Logging.Log(Tags.DRONE_FEEDBACK, $"{_parentCruiserHasActiveDrones.Value}");

            if (_parentCruiserHasActiveDrones.Value)
            {
                _audioSource.Play(isSpatial: true);
            }
        }

        private void _settingsManager_SettingsSaved(object sender, EventArgs e)
        {
            SetVolume();
        }

        private void SetVolume()
        {
            _audioSource.Volume = _settingsManager.EffectVolume;
        }

        public void DisposeManagedState()
        {
            _parentCruiserHasActiveDrones.ValueChanged -= _parentCruiserHasActiveDrones_ValueChanged;
            _settingsManager.SettingsSaved -= _settingsManager_SettingsSaved;
        }
    }
}