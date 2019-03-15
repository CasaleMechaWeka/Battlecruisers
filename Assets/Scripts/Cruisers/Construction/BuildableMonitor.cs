using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using System.Collections.ObjectModel;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Construction
{
    // FELIX  Test, use :)
    public class BuildableMonitor : IBuildableMonitor
    {
        private readonly ICruiserController _cruiser;

        //public IReadOnlyCollection

        public ReadOnlyCollection<IBuilding> AliveBuildings { get; private set; }
        public ReadOnlyCollection<IUnit> AliveUnits { get; private set; }

        public BuildableMonitor(ICruiserController cruiser)
        {
            Assert.IsNotNull(cruiser);

            _cruiser = cruiser;
            _cruiser.BuildingCompleted += _cruiser_BuildingCompleted;
            _cruiser.CompletedBuildingUnit += _cruiser_CompletedBuildingUnit;
        }

        private void _cruiser_BuildingCompleted(object sender, CompletedBuildingConstructionEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void _cruiser_CompletedBuildingUnit(object sender, CompletedUnitConstructionEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}