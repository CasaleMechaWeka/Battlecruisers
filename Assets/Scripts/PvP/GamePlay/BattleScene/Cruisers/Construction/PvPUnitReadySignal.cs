using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction
{
    public class PvPUnitReadySignal : IPvPManagedDisposable
    {
        private readonly IPvPCruiserUnitMonitor _unitMonitor;
        private readonly IAudioSource _navalAudioSource, _aircraftAudioSource;

        public PvPUnitReadySignal(IPvPCruiserUnitMonitor unitMonitor, IAudioSource navalAudioSource, IAudioSource aircraftAudioSource)
        {
            PvPHelper.AssertIsNotNull(unitMonitor, navalAudioSource, aircraftAudioSource);

            _unitMonitor = unitMonitor;
            _navalAudioSource = navalAudioSource;
            _aircraftAudioSource = aircraftAudioSource;

            _unitMonitor.UnitCompleted += _unitMonitor_UnitCompleted;
        }

        private void _unitMonitor_UnitCompleted(object sender, PvPUnitCompletedEventArgs e)
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