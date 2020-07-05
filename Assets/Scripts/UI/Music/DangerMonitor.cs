using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
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

            _playerCruiser.BuildingMonitor.BuildingCompleted += Cruiser_BuildingCompleted;
            _playerCruiser.UnitMonitor.UnitCompleted += Cruiser_CompletedBuildingUnit;
            _aiCruiser.BuildingMonitor.BuildingCompleted += Cruiser_BuildingCompleted;
            _aiCruiser.UnitMonitor.UnitCompleted += Cruiser_CompletedBuildingUnit;

            _playerCruiserHealthMonitor.DroppedBelowThreshold += CruiserHealthMonitor_DroppedBelowThreshold;
            _aiCruiserHealthMonitor.DroppedBelowThreshold += CruiserHealthMonitor_DroppedBelowThreshold;
        }

        private void Cruiser_BuildingCompleted(object sender, BuildingCompletedEventArgs e)
        {
            if (e.CompletedBuilding.Category == BuildingCategory.Offence
                || e.CompletedBuilding.Category == BuildingCategory.Ultra)
            {
                EmitDanger();
            }
        }

        private void Cruiser_CompletedBuildingUnit(object sender, UnitCompletedEventArgs e)
        {
            if (e.CompletedUnit.IsUltra)
            {
                EmitDanger();
            }
        }

        private void CruiserHealthMonitor_DroppedBelowThreshold(object sender, EventArgs e)
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