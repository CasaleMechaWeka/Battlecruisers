using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
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
        private readonly IDeferrer _timeScaleDeferrer;
        private readonly ICruiserController _playerCruiser, _aiCruiser;
        private readonly IHealthThresholdMonitor _playerCruiserHealthMonitor, _aiCruiserHealthMonitor;
        
        public const float DANGER_LIFETIME_IN_S = 15;

        public event EventHandler DangerStart;
        public event EventHandler DangerEnd;

        public DangerMonitor(
            IDeferrer timeScaleDeferrer,
            ICruiserController playerCruiser, 
            ICruiserController aiCruiser,
            IHealthThresholdMonitor playerCruiserHealthMonitor,
            IHealthThresholdMonitor aiCruiserHealthMonitor)
        {
            Helper.AssertIsNotNull(timeScaleDeferrer, timeScaleDeferrer, playerCruiser, aiCruiser, playerCruiserHealthMonitor, aiCruiserHealthMonitor);

            _timeScaleDeferrer = timeScaleDeferrer;
            _playerCruiser = playerCruiser;
            _aiCruiser = aiCruiser;
            _playerCruiserHealthMonitor = playerCruiserHealthMonitor;
            _aiCruiserHealthMonitor = aiCruiserHealthMonitor;

            _playerCruiser.BuildingMonitor.BuildingCompleted += Cruiser_BuildingCompleted;
            _playerCruiser.UnitMonitor.UnitCompleted += Cruiser_CompletedBuildingUnit;
            _aiCruiser.BuildingMonitor.BuildingCompleted += Cruiser_BuildingCompleted;
            _aiCruiser.UnitMonitor.UnitCompleted += Cruiser_CompletedBuildingUnit;

            _playerCruiserHealthMonitor.DroppedBelowThreshold += CruiserHealthMonitor_DroppedBelowThreshold;
            _playerCruiserHealthMonitor.RoseAboveThreshold += CruiserHealthMonitor_RoseAboveThreshold;
            _aiCruiserHealthMonitor.DroppedBelowThreshold += CruiserHealthMonitor_DroppedBelowThreshold;
            _aiCruiserHealthMonitor.RoseAboveThreshold += CruiserHealthMonitor_RoseAboveThreshold;
        }

        private void Cruiser_BuildingCompleted(object sender, BuildingCompletedEventArgs e)
        {
            if (e.CompletedBuilding.Category == BuildingCategory.Offence
                || e.CompletedBuilding.Category == BuildingCategory.Ultra)
            {
                EmitDangerStart();
            }
        }

        private void Cruiser_CompletedBuildingUnit(object sender, UnitCompletedEventArgs e)
        {
            if (e.CompletedUnit.IsUltra)
            {
                EmitDangerStart();
            }
        }

        private void CruiserHealthMonitor_DroppedBelowThreshold(object sender, EventArgs e)
        {
            if (_playerCruiser.IsAlive
                && _aiCruiser.IsAlive)
            {
                EmitDangerStart(deferDangerEnd: false);
            }
        }

        private void CruiserHealthMonitor_RoseAboveThreshold(object sender, EventArgs e)
        {
            EmitDangerEnd();
        }

        private void EmitDangerStart(bool deferDangerEnd = true)
        {
            DangerStart?.Invoke(this, EventArgs.Empty);

            if (deferDangerEnd)
            {
                _timeScaleDeferrer.Defer(EmitDangerEnd, DANGER_LIFETIME_IN_S);
            }
        }

        private void EmitDangerEnd()
        {
            DangerEnd?.Invoke(this, EventArgs.Empty);
        }
    }
}