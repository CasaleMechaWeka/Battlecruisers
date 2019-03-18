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
    public class UnitConstructionMonitor : IUnitConstructionMonitor, IManagedDisposable
    {
        private readonly ICruiserController _cruiser;

        public IReadOnlyCollection<IUnit> AliveUnits => throw new NotImplementedException();

        public event EventHandler<StartedUnitConstructionEventArgs> StartedBuildingUnit;
        public event EventHandler<CompletedUnitConstructionEventArgs> CompletedBuildingUnit;
        public event EventHandler<UnitDestroyedEventArgs> UnitDestroyed;

        public UnitConstructionMonitor(ICruiserController cruiser)
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
            StartedBuildingUnit?.Invoke(this, e);
        }

        private void Factory_CompletedBuildingUnit(object sender, CompletedUnitConstructionEventArgs e)
        {
            CompletedBuildingUnit?.Invoke(this, e);
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