using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Construction
{
    // FELIX  Test :)
    public class UnitConstructionMonitor : IUnitConstructionMonitor, IManagedDisposable
    {
        private readonly ICruiserController _cruiser;

        public event EventHandler<StartedUnitConstructionEventArgs> StartedBuildingUnit;

        public UnitConstructionMonitor(ICruiserController cruiser)
        {
            Assert.IsNotNull(cruiser);

            _cruiser = cruiser;
            _cruiser.BuildingStarted += _cruiser_BuildingStarted;
        }

        private void _cruiser_BuildingStarted(object sender, StartedBuildingConstructionEventArgs e)
        {
            IFactory factory = e.Buildable as IFactory;

            if (factory != null)
            {
                factory.StartedBuildingUnit += Factory_StartedBuildingUnit;
                factory.Destroyed += Factory_Destroyed;
            }
        }

        private void Factory_StartedBuildingUnit(object sender, StartedUnitConstructionEventArgs e)
        {
            if (StartedBuildingUnit != null)
            {
                StartedBuildingUnit.Invoke(this, e);
            }
        }

        private void Factory_Destroyed(object sender, DestroyedEventArgs e)
        {
            IFactory factory = e.DestroyedTarget.Parse<IFactory>();
            factory.StartedBuildingUnit -= Factory_StartedBuildingUnit;
            factory.Destroyed -= Factory_Destroyed;
        }

        public void DisposeManagedState()
        {
            _cruiser.BuildingStarted -= _cruiser_BuildingStarted;
        }
    }
}