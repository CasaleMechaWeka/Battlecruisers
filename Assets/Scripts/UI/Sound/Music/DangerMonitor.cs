using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
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
    /// FELIX  Test :)
    public class DangerMonitor : IDangerMonitor
    {
        private readonly ICruiserController _playerCruiser, _aiCruiser;

        public event EventHandler Danger;
        
        public DangerMonitor(ICruiserController playerCruiser, ICruiserController aiCruiser)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser);

            _playerCruiser = playerCruiser;
            _aiCruiser = aiCruiser;

            // FELIX  NEXT:  Cruiser health monitors :)

            _playerCruiser.BuildingCompleted += _playerCruiser_BuildingCompleted;
            _playerCruiser.CompletedBuildingUnit += _playerCruiser_CompletedBuildingUnit;
        }

        private void _playerCruiser_BuildingCompleted(object sender, CompletedBuildingConstructionEventArgs e)
        {
            if (e.Buildable.Category == BuildingCategory.Offence
                || e.Buildable.Category == BuildingCategory.Ultra)
            {
                EmitDanger();
            }
        }

        private void _playerCruiser_CompletedBuildingUnit(object sender, CompletedUnitConstructionEventArgs e)
        {
            if (e.Buildable.IsUltra)
            {
                EmitDanger();
            }
        }

        private void EmitDanger()
        {
            if (Danger != null)
            {
                Danger.Invoke(this, EventArgs.Empty);
            }
        }
    }
}