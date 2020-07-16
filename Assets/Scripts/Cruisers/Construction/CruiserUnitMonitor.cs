using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Construction
{
    public class CruiserUnitMonitor : ICruiserUnitMonitor, IManagedDisposable
    {
        private readonly ICruiserBuildingMonitor _buildingMonitor;

        private readonly HashSet<IUnit> _aliveUnits;
        public IReadOnlyCollection<IUnit> AliveUnits => _aliveUnits;

        public event EventHandler<UnitStartedEventArgs> UnitStarted;
        public event EventHandler<UnitCompletedEventArgs> UnitCompleted;
        public event EventHandler<UnitDestroyedEventArgs> UnitDestroyed;

        public CruiserUnitMonitor(ICruiserBuildingMonitor buildingMonitor)
        {
            Assert.IsNotNull(buildingMonitor);

            _buildingMonitor = buildingMonitor;
            _buildingMonitor.BuildingCompleted += _buildingMonitor_BuildingCompleted;

            _aliveUnits = new HashSet<IUnit>();
        }

        private void _buildingMonitor_BuildingCompleted(object sender, BuildingCompletedEventArgs e)
        {
            IFactory factory = e.CompletedBuilding as IFactory;

            if (factory != null)
            {
                factory.UnitStarted += Factory_StartedBuildingUnit;
                factory.UnitCompleted += Factory_CompletedBuildingUnit;
                factory.Destroyed += Factory_Destroyed;
            }
        }

        private void Factory_StartedBuildingUnit(object sender, UnitStartedEventArgs e)
        {
            Logging.Log(Tags.UNIT_MONITOR, $"_aliveUnits.Count: {_aliveUnits.Count}  e.StartedUnit: {e.StartedUnit}");

            Assert.IsFalse(_aliveUnits.Contains(e.StartedUnit));
            _aliveUnits.Add(e.StartedUnit);

            e.StartedUnit.Destroyed += Unit_Destroyed;

            UnitStarted?.Invoke(this, e);
        }

        private void Factory_CompletedBuildingUnit(object sender, UnitCompletedEventArgs e)
        {
            UnitCompleted?.Invoke(this, e);
        }

        private void Unit_Destroyed(object sender, DestroyedEventArgs e)
        {
            Logging.Log(Tags.UNIT_MONITOR, $"_aliveUnits.Count: {_aliveUnits.Count}  e.DestroyedTarget: {e.DestroyedTarget}");

            IUnit destroyedUnit = e.DestroyedTarget.Parse<IUnit>();
            destroyedUnit.Destroyed -= Unit_Destroyed;

            Assert.IsTrue(_aliveUnits.Contains(destroyedUnit));
            _aliveUnits.Remove(destroyedUnit);

            UnitDestroyed?.Invoke(this, new UnitDestroyedEventArgs(destroyedUnit));
        }

        private void Factory_Destroyed(object sender, DestroyedEventArgs e)
        {
            IFactory factory = e.DestroyedTarget.Parse<IFactory>();
            factory.UnitStarted -= Factory_StartedBuildingUnit;
            factory.UnitCompleted -= Factory_CompletedBuildingUnit;
            factory.Destroyed -= Factory_Destroyed;
        }

        public void DisposeManagedState()
        {
            _buildingMonitor.BuildingCompleted -= _buildingMonitor_BuildingCompleted;
        }
    }
}