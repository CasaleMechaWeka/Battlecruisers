using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction
{
    public class PvPUnitReadySignal : IPvPManagedDisposable
    {
        private readonly IPvPCruiserUnitMonitor _unitMonitor;
        private readonly IPvPAudioSource _navalAudioSource, _aircraftAudioSource;

        public PvPUnitReadySignal(IPvPCruiserUnitMonitor unitMonitor, IPvPAudioSource navalAudioSource, IPvPAudioSource aircraftAudioSource)
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
                case PvPTargetType.Aircraft:
                    _aircraftAudioSource.Play();
                    break;

                case PvPTargetType.Ships:
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