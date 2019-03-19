using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers.Construction
{
    public class UltrasConstructionMonitor : IManagedDisposable
    {
        private readonly ICruiserBuildingMonitor _cruiserBuildingMonitor;
        private readonly ICruiserUnitMonitor _cruiserUnitMonitor;
        private readonly IPrioritisedSoundPlayer _soundPlayer;

        public UltrasConstructionMonitor(ICruiserController cruiser, IPrioritisedSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(cruiser, soundPlayer);

            _cruiserBuildingMonitor = cruiser.BuildingMonitor;
            _cruiserUnitMonitor = cruiser.UnitMonitor;
            _soundPlayer = soundPlayer;

            _cruiserBuildingMonitor.BuildingStarted += _buildingMonitor_BuildingStarted;
            _cruiserUnitMonitor.UnitStarted += _unitMonitor_StartedBuildingUnit;
        }

        private void _buildingMonitor_BuildingStarted(object sender, BuildingStartedEventArgs e)
        {
            if (e.StartedBuilding.Category == BuildingCategory.Ultra)
            {
                _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.EnemyStartedUltra);
            }
        }

        private void _unitMonitor_StartedBuildingUnit(object sender, UnitStartedEventArgs e)
        {
            if (e.StartedUnit.IsUltra)
            {
                _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.EnemyStartedUltra);
            }
        }

        public void DisposeManagedState()
        {
            _cruiserBuildingMonitor.BuildingStarted -= _buildingMonitor_BuildingStarted;
            _cruiserUnitMonitor.UnitStarted -= _unitMonitor_StartedBuildingUnit;
        }
    }
}