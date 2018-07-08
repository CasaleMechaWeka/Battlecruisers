using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Tasks
{
    /// <summary>
    /// Waits for whichever is sooner:
    /// 1. Specified number of units are completed
    /// 2. Factory is destroyed
    /// 3. Factory has all its drones removed (ie, no unit is being built)
    /// </summary>
    public class WaitForUnitConstructionTask : IInternalTask
    {
        private readonly IFactory _factory;
        private readonly int _numOfUnitsToBuild;
        private int _numOfUnitsBuilt;

        private const int DEFAULT_NUM_OF_UNIT = 3;

        public event EventHandler Completed;

        private bool FactoryCanProduceUnit
        {
            get
            {
                return
                    !_factory.IsDestroyed
                    && _factory.BuildableState == BuildableState.Completed
                    && _factory.UnitWrapper != null
                    && _factory.NumOfDrones != 0;
            }
        }

        public WaitForUnitConstructionTask(IFactory factory, int numOfUnitsToBuild = DEFAULT_NUM_OF_UNIT)
        {
            Assert.IsNotNull(factory);

            _factory = factory;
            _numOfUnitsToBuild = numOfUnitsToBuild;
            _numOfUnitsBuilt = 0;
        }

        public void Start()
        {
            if (!FactoryCanProduceUnit)
            {
                Complete();
            }

            _factory.CompletedBuildingUnit += _factory_CompletedBuildingUnit;
            _factory.Destroyed += _factory_Destroyed;
            _factory.DroneNumChanged += _factory_DroneNumChanged;
        }

        private void _factory_CompletedBuildingUnit(object sender, CompletedConstructionEventArgs e)
        {
            _numOfUnitsBuilt++;

            if (_numOfUnitsBuilt >= _numOfUnitsToBuild)
            {
                Complete();
            }
        }

        private void _factory_Destroyed(object sender, DestroyedEventArgs e)
        {
            Complete();
        }

        private void _factory_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            if (e.NewNumOfDrones == 0)
            {
                // Factory has stopped building units.  Don't want to wait indefinitely,
                // so simply complete :)
                Complete();
            }
        }

        private void Complete()
        {
            if (Completed != null)
            {
                Completed.Invoke(this, EventArgs.Empty);
            }

            _factory.CompletedBuildingUnit -= _factory_CompletedBuildingUnit;
            _factory.Destroyed -= _factory_Destroyed;
            _factory.DroneNumChanged -= _factory_DroneNumChanged;
        }

        public void Stop()
        {
            // Empty
        }

        public void Resume()
        {
            // Empty
        }
    }
}
