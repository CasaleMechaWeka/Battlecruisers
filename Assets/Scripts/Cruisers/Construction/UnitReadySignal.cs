using BattleCruisers.Buildables;
using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;

namespace BattleCruisers.Cruisers.Construction
{
    // FELIX  Update tests
    public class UnitReadySignal : IManagedDisposable
    {
        private readonly ICruiserUnitMonitor _unitMonitor;
        private readonly IAudioSource _navalAudioSource, _aircraftAudioSource;
        private readonly ISettingsManager _settingsManager;

        public UnitReadySignal(
            ICruiserUnitMonitor unitMonitor, 
            IAudioSource navalAudioSource, 
            IAudioSource aircraftAudioSource,
            ISettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(unitMonitor, navalAudioSource, aircraftAudioSource, settingsManager);

            _unitMonitor = unitMonitor;
            _navalAudioSource = navalAudioSource;
            _aircraftAudioSource = aircraftAudioSource;
            _settingsManager = settingsManager;

            SetVolume();

            _unitMonitor.UnitCompleted += _unitMonitor_UnitCompleted;
            _settingsManager.SettingsSaved += _settingsManager_SettingsSaved;
        }

        private void _unitMonitor_UnitCompleted(object sender, UnitCompletedEventArgs e)
        {
            switch (e.CompletedUnit.TargetType)
            {
                case TargetType.Aircraft:
                    _aircraftAudioSource.Play();
                    break;

                case TargetType.Ships:
                    _navalAudioSource.Play();
                    break;

                default:
                    throw new ArgumentException($"Unsupported target type: {e.CompletedUnit.TargetType}");
            }
        }

        private void _settingsManager_SettingsSaved(object sender, EventArgs e)
        {
            SetVolume();
        }

        private void SetVolume()
        {
            _navalAudioSource.Volume = _settingsManager.EffectVolume;
            _aircraftAudioSource.Volume = _settingsManager.EffectVolume;
        }

        public void DisposeManagedState()
        {
            _unitMonitor.UnitCompleted -= _unitMonitor_UnitCompleted;
        }
    }
}