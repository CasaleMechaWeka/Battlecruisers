using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Construction
{
    // FELIX  Test, use :)
    public class BuildableMonitor : IBuildableMonitor
    {
        private readonly ICruiserController _cruiser;
        
        private readonly HashSet<IBuilding> _aliveBuildings;
        public IReadOnlyCollection<IBuilding> AliveBuildings => _aliveBuildings;

        private readonly HashSet<IUnit> _aliveUnits;
        public IReadOnlyCollection<IUnit> AliveUnits => _aliveUnits;

        public BuildableMonitor(ICruiserController cruiser)
        {
            Assert.IsNotNull(cruiser);

            _cruiser = cruiser;
            _cruiser.BuildingCompleted += _cruiser_BuildingCompleted;
            _cruiser.CompletedBuildingUnit += _cruiser_CompletedBuildingUnit;

            _aliveBuildings = new HashSet<IBuilding>();
            _aliveUnits = new HashSet<IUnit>();
        }

        private void _cruiser_BuildingCompleted(object sender, CompletedBuildingConstructionEventArgs e)
        {
            Assert.IsFalse(_aliveBuildings.Contains(e.Buildable));
            _aliveBuildings.Add(e.Buildable);
            e.Buildable.Destroyed += Building_Destroyed;
        }

        private void Building_Destroyed(object sender, DestroyedEventArgs e)
        {
            IBuilding destroyedBuilding = e.DestroyedTarget as IBuilding;
            Assert.IsNotNull(destroyedBuilding);

            Assert.IsTrue(_aliveBuildings.Contains(destroyedBuilding));
            _aliveBuildings.Remove(destroyedBuilding);
            destroyedBuilding.Destroyed -= Building_Destroyed;
        }

        private void _cruiser_CompletedBuildingUnit(object sender, CompletedUnitConstructionEventArgs e)
        {
            Assert.IsFalse(_aliveUnits.Contains(e.Buildable));
            _aliveUnits.Add(e.Buildable);
            e.Buildable.Destroyed += Unit_Destroyed;
        }

        private void Unit_Destroyed(object sender, DestroyedEventArgs e)
        {
            IUnit destroyedUnit = e.DestroyedTarget as IUnit;
            Assert.IsNotNull(destroyedUnit);

            Assert.IsTrue(_aliveUnits.Contains(destroyedUnit));
            _aliveUnits.Remove(destroyedUnit);
            destroyedUnit.Destroyed -= Unit_Destroyed;
        }
    }
}