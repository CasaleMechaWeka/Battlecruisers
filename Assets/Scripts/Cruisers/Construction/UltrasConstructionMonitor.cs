using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers.Construction
{
    public class UltrasConstructionMonitor : IManagedDisposable
    {
        private readonly ICruiserController _cruiser;
        private readonly IPrioritisedSoundPlayer _soundPlayer;

        public UltrasConstructionMonitor(ICruiserController cruiser, IPrioritisedSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(cruiser, soundPlayer);

            _cruiser = cruiser;
            _soundPlayer = soundPlayer;

            _cruiser.BuildingStarted += _cruiser_BuildingStarted;
            _cruiser.UnitStarted += _unitConstructionMonitor_StartedBuildingUnit;
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
            _cruiser.UnitStarted -= _unitConstructionMonitor_StartedBuildingUnit;
        }
    }
}