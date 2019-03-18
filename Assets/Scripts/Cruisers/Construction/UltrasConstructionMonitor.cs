using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers.Construction
{
    // FELIX  Update tests :)
    public class UltrasConstructionMonitor : IManagedDisposable
    {
        // FELIX  Replace with ICruiserBuildingMonitor (once it exists :P)
        private readonly ICruiserController _cruiser;
        private readonly ICruiserUnitMonitor _cruiserUnitMonitor;
        private readonly IPrioritisedSoundPlayer _soundPlayer;

        public UltrasConstructionMonitor(ICruiserController cruiser, IPrioritisedSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(cruiser, soundPlayer);

            _cruiser = cruiser;
            _cruiserUnitMonitor = cruiser.UnitMonitor;
            _soundPlayer = soundPlayer;

            _cruiser.BuildingStarted += _cruiser_BuildingStarted;
            _cruiserUnitMonitor.UnitStarted += _unitConstructionMonitor_StartedBuildingUnit;
        }

        private void _cruiser_BuildingStarted(object sender, StartedBuildingConstructionEventArgs e)
        {
            if (e.Buildable.Category == BuildingCategory.Ultra)
            {
                _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.EnemyStartedUltra);
            }
        }

        private void _unitConstructionMonitor_StartedBuildingUnit(object sender, StartedUnitConstructionEventArgs e)
        {
            if (e.Buildable.IsUltra)
            {
                _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.EnemyStartedUltra);
            }
        }

        public void DisposeManagedState()
        {
            _cruiser.BuildingStarted -= _cruiser_BuildingStarted;
            _cruiserUnitMonitor.UnitStarted -= _unitConstructionMonitor_StartedBuildingUnit;
        }
    }
}