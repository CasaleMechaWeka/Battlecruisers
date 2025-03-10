using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;

namespace BattleCruisers.Cruisers.Construction
{
    public class UnitReadySignal : IManagedDisposable
    {
        private readonly ICruiserUnitMonitor _unitMonitor;
        private readonly IAudioSource _navalAudioSource, _aircraftAudioSource;

        public UnitReadySignal(ICruiserUnitMonitor unitMonitor, IAudioSource navalAudioSource, IAudioSource aircraftAudioSource)
        {
            Helper.AssertIsNotNull(unitMonitor, navalAudioSource, aircraftAudioSource);

            _unitMonitor = unitMonitor;
            _navalAudioSource = navalAudioSource;
            _aircraftAudioSource = aircraftAudioSource;

            _unitMonitor.UnitCompleted += _unitMonitor_UnitCompleted;
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

        public void DisposeManagedState()
        {
            _unitMonitor.UnitCompleted -= _unitMonitor_UnitCompleted;
        }
    }
}