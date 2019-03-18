using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Construction
{
    // FELIX  Update test :)
    public class CruiserUnitMonitor : ICruiserUnitMonitor, IManagedDisposable
    {
        private readonly ICruiserController _cruiser;

        private readonly HashSet<IUnit> _aliveUnits;
        public IReadOnlyCollection<IUnit> AliveUnits => _aliveUnits;

        public event EventHandler<StartedUnitConstructionEventArgs> UnitStarted;
        public event EventHandler<CompletedUnitConstructionEventArgs> UnitCompleted;
        public event EventHandler<UnitDestroyedEventArgs> UnitDestroyed;

        // FELIX  Will have to replace with ICruiserBuildingMonitor :)
        public CruiserUnitMonitor(ICruiserController cruiser)
        {
            Assert.IsNotNull(cruiser);

            _cruiser = cruiser;
            _cruiser.BuildingCompleted += _cruiser_BuildingCompleted;
        }

        private void _cruiser_BuildingCompleted(object sender, CompletedBuildingConstructionEventArgs e)
        {
            IFactory factory = e.Buildable as IFactory;

            if (factory != null)
            {
                factory.StartedBuildingUnit += Factory_StartedBuildingUnit;
                factory.CompletedBuildingUnit += Factory_CompletedBuildingUnit;
                factory.Destroyed += Factory_Destroyed;
            }
        }

        private void Factory_StartedBuildingUnit(object sender, StartedUnitConstructionEventArgs e)
        {
            UnitStarted?.Invoke(this, e);
        }

        private void Factory_CompletedBuildingUnit(object sender, CompletedUnitConstructionEventArgs e)
        {
            UnitCompleted?.Invoke(this, e);

            Assert.IsFalse(_aliveUnits.Contains(e.Buildable));
            _aliveUnits.Add(e.Buildable);
            e.Buildable.Destroyed += Unit_Destroyed;
        }

        private void Unit_Destroyed(object sender, DestroyedEventArgs e)
        {
            IUnit destroyedUnit = e.DestroyedTarget.Parse<IUnit>();

            Assert.IsTrue(_aliveUnits.Contains(destroyedUnit));
            _aliveUnits.Remove(destroyedUnit);
            destroyedUnit.Destroyed -= Unit_Destroyed;

            UnitDestroyed?.Invoke(this, new UnitDestroyedEventArgs(destroyedUnit));
        }

        private void Factory_Destroyed(object sender, DestroyedEventArgs e)
        {
            IFactory factory = e.DestroyedTarget.Parse<IFactory>();
            factory.StartedBuildingUnit -= Factory_StartedBuildingUnit;
            factory.CompletedBuildingUnit -= Factory_CompletedBuildingUnit;
            factory.Destroyed -= Factory_Destroyed;
        }

        public void DisposeManagedState()
        {
            _cruiser.BuildingCompleted -= _cruiser_BuildingCompleted;
        }
    }
}