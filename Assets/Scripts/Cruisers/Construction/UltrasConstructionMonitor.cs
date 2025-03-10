using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;

namespace BattleCruisers.Cruisers.Construction
{
    public class UltrasConstructionMonitor : IManagedDisposable
    {
        private readonly ICruiserBuildingMonitor _cruiserBuildingMonitor;
        private readonly ICruiserUnitMonitor _cruiserUnitMonitor;
        private readonly IPrioritisedSoundPlayer _soundPlayer;
        private readonly IDebouncer _debouncer;

        public UltrasConstructionMonitor(
            ICruiserController cruiser, 
            IPrioritisedSoundPlayer soundPlayer,
            IDebouncer debouncer)
        {
            Helper.AssertIsNotNull(cruiser, soundPlayer, debouncer);

            _cruiserBuildingMonitor = cruiser.BuildingMonitor;
            _cruiserUnitMonitor = cruiser.UnitMonitor;
            _soundPlayer = soundPlayer;
            _debouncer = debouncer;

            _cruiserBuildingMonitor.BuildingStarted += _buildingMonitor_BuildingStarted;
            _cruiserUnitMonitor.UnitStarted += _unitMonitor_StartedBuildingUnit;
        }

        private void _buildingMonitor_BuildingStarted(object sender, BuildingStartedEventArgs e)
        {
            if (e.StartedBuilding.Category == BuildingCategory.Ultra)
            {
                _debouncer.Debounce(PlayAlert);
            }
        }

        private void _unitMonitor_StartedBuildingUnit(object sender, UnitStartedEventArgs e)
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