using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.Music
{
    /// <summary>
    /// Emits a Danger event in the following circumstances:
    /// + An offensive is completed
    /// + An ultra is completed
    /// + A cruiser drops below 1/3 health
    /// </summary>
    public class DangerMonitor : IDangerMonitor
    {
        private readonly ICruiserController _playerCruiser, _aiCruiser;
        private readonly IHealthThresholdMonitor _playerCruiserHealthMonitor, _aiCruiserHealthMonitor;

        public event EventHandler Danger;
        
        public DangerMonitor(
            ICruiserController playerCruiser, 
            ICruiserController aiCruiser,
            IHealthThresholdMonitor playerCruiserHealthMonitor,
            IHealthThresholdMonitor aiCruiserHealthMonitor)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, playerCruiserHealthMonitor, aiCruiserHealthMonitor);

            _playerCruiser = playerCruiser;
            _aiCruiser = aiCruiser;
            _playerCruiserHealthMonitor = playerCruiserHealthMonitor;
            _aiCruiserHealthMonitor = aiCruiserHealthMonitor;

            _playerCruiser.BuildingCompleted += Cruiser_BuildingCompleted;
            _playerCruiser.CompletedBuildingUnit += Cruiser_CompletedBuildingUnit;
            _aiCruiser.BuildingCompleted += Cruiser_BuildingCompleted;
            _aiCruiser.CompletedBuildingUnit += Cruiser_CompletedBuildingUnit;

            _playerCruiserHealthMonitor.ThresholdReached += CruiserHealthMonitor_ThresholdReached;
            _aiCruiserHealthMonitor.ThresholdReached += CruiserHealthMonitor_ThresholdReached;
        }

        private void Cruiser_BuildingCompleted(object sender, CompletedBuildingConstructionEventArgs e)
        {
            if (e.Buildable.Category == BuildingCategory.Offence
                || e.Buildable.Category == BuildingCategory.Ultra)
            {
                EmitDanger();
            }
        }

        private void Cruiser_CompletedBuildingUnit(object sender, CompletedUnitConstructionEventArgs e)
        {
            if (e.Buildable.IsUltra)
            {
                EmitDanger();
            }
        }

        private void CruiserHealthMonitor_ThresholdReached(object sender, EventArgs e)
        {
            if (_playerCruiser.IsAlive
                && _aiCruiser.IsAlive)
            {
                EmitDanger();
            }
        }

        private void EmitDanger()
        {
            Danger?.Invoke(this, EventArgs.Empty);
        }
    }
}