using BattleCruisers.Data.Static;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction
{
    public class PvPUltrasConstructionMonitor : IManagedDisposable
    {
        private readonly IPvPCruiserBuildingMonitor _cruiserBuildingMonitor;
        private readonly IPvPCruiserUnitMonitor _cruiserUnitMonitor;
        private readonly IPvPPrioritisedSoundPlayer _soundPlayer;
        private readonly IDebouncer _debouncer;

        public PvPUltrasConstructionMonitor(
            IPvPCruiserController cruiser,
            IPvPPrioritisedSoundPlayer soundPlayer,
            IDebouncer debouncer)
        {
            PvPHelper.AssertIsNotNull(cruiser, soundPlayer, debouncer);

            _cruiserBuildingMonitor = cruiser.BuildingMonitor;
            _cruiserUnitMonitor = cruiser.UnitMonitor;
            _soundPlayer = soundPlayer;
            _debouncer = debouncer;

            _cruiserBuildingMonitor.BuildingStarted += _buildingMonitor_BuildingStarted;
            _cruiserUnitMonitor.UnitStarted += _unitMonitor_StartedBuildingUnit;
        }

        private void _buildingMonitor_BuildingStarted(object sender, PvPBuildingStartedEventArgs e)
        {
            if (e.StartedBuilding.Category == BuildingCategory.Ultra)
            {
                _debouncer.Debounce(PlayAlert);
            }
        }

        private void _unitMonitor_StartedBuildingUnit(object sender, PvPUnitStartedEventArgs e)
        {
            if (e.StartedUnit.IsUltra)
            {
                _debouncer.Debounce(PlayAlert);
            }
        }

        private void PlayAlert()
        {
            _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.EnemyStartedUltra);
        }

        public void DisposeManagedState()
        {
            _cruiserBuildingMonitor.BuildingStarted -= _buildingMonitor_BuildingStarted;
            _cruiserUnitMonitor.UnitStarted -= _unitMonitor_StartedBuildingUnit;
        }
    }
}