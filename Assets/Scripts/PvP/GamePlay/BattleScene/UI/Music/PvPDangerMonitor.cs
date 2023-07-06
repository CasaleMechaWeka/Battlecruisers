using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music
{
    /// <summary>
    /// Emits a Danger event in the following circumstances:
    /// + An offensive is completed
    /// + An ultra is completed
    /// + A cruiser drops below 1/3 health
    /// </summary>
    public class PvPDangerMonitor : IPvPDangerMonitor
    {
        private readonly IPvPDeferrer _timeScaleDeferrer;
        private readonly IPvPCruiserController _playerCruiser, _aiCruiser;
        private readonly IPvPHealthThresholdMonitor _playerCruiserHealthMonitor, _aiCruiserHealthMonitor;

        public const float DANGER_LIFETIME_IN_S = 60;

        public event EventHandler DangerStart;
        public event EventHandler DangerEnd;

        public PvPDangerMonitor(
            IPvPDeferrer timeScaleDeferrer,
            IPvPCruiserController playerCruiser,
            IPvPCruiserController aiCruiser,
            IPvPHealthThresholdMonitor playerCruiserHealthMonitor,
            IPvPHealthThresholdMonitor aiCruiserHealthMonitor)
        {
            PvPHelper.AssertIsNotNull(timeScaleDeferrer, timeScaleDeferrer, playerCruiser, aiCruiser, playerCruiserHealthMonitor, aiCruiserHealthMonitor);

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

        private void Cruiser_BuildingCompleted(object sender, PvPBuildingCompletedEventArgs e)
        {
            if (e.CompletedBuilding.Category == PvPBuildingCategory.Offence
                || e.CompletedBuilding.Category == PvPBuildingCategory.Ultra)
            {
                EmitDangerStart();
            }
        }

        private void Cruiser_CompletedBuildingUnit(object sender, PvPUnitCompletedEventArgs e)
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