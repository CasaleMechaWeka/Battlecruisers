using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.InGameHints
{
    public class BuildingMonitor : IBuildingMonitor
    {
        private readonly ICruiserController _cruiser;

        public event EventHandler AirFactoryStarted;
        public event EventHandler NavalFactoryStarted;
        public event EventHandler OffensiveStarted;
        public event EventHandler AirDefensiveStarted;
        public event EventHandler ShipDefensiveStarted;
        public event EventHandler ShieldStarted;

        public BuildingMonitor(ICruiserController cruiser)
        {
            Assert.IsNotNull(cruiser);

            _cruiser = cruiser;
            _cruiser.BuildingStarted += _cruiser_BuildingStarted;
        }

        private void _cruiser_BuildingStarted(object sender, BuildingStartedEventArgs e)
        {
            if (e.StartedBuilding is AirFactory)
            {
                AirFactoryStarted?.Invoke(this, EventArgs.Empty);
            }
            else if (e.StartedBuilding is NavalFactory)
            {
                NavalFactoryStarted?.Invoke(this, EventArgs.Empty);
            }
            else if (e.StartedBuilding.Category == BuildingCategory.Offence)
            {
                OffensiveStarted?.Invoke(this, EventArgs.Empty);
            }
            else if (e.StartedBuilding.Category == BuildingCategory.Defence)
            {
                if (e.StartedBuilding.SlotSpecification.BuildingFunction == BuildingFunction.AntiAir)
                {
                    AirDefensiveStarted?.Invoke(this, EventArgs.Empty);
                }
                else if (e.StartedBuilding.SlotSpecification.BuildingFunction == BuildingFunction.AntiShip)
                {
                    ShipDefensiveStarted?.Invoke(this, EventArgs.Empty);
                }
            }
            else if (e.StartedBuilding.SlotSpecification.BuildingFunction == BuildingFunction.Shield)
            {
                ShieldStarted?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}