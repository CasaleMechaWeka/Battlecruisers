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
            IFactory factory = e.Buildable as IFactory;

            if (factory != null)
            {
                factory.StartedBuildingUnit += Factory_StartedBuildingUnit;
                factory.CompletedBuildingUnit += Factory_CompletedBuildingUnit;
                factory.Destroyed += Factory_Destroyed;
            }
        }

        private void Factory_StartedBuildingUnit(object sender, UnitStartedEventArgs e)
        {
            UnitStarted?.Invoke(this, e);
            e.Buildable.Destroyed += Unit_Destroyed;
        }

        private void Factory_CompletedBuildingUnit(object sender, UnitCompletedEventArgs e)
        {
            UnitCompleted?.Invoke(this, e);

            Assert.IsFalse(_aliveUnits.Contains(e.Buildable));
            _aliveUnits.Add(e.Buildable);
        }

        private void Unit_Destroyed(object sender, DestroyedEventArgs e)
        {
            IUnit destroyedUnit = e.DestroyedTarget.Parse<IUnit>();
            destroyedUnit.Destroyed -= Unit_Destroyed;

            if (_aliveUnits.Contains(destroyedUnit))
            {
                _aliveUnits.Remove(destroyedUnit);
            }

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
            _buildingMonitor.BuildingCompleted -= _buildingMonitor_BuildingCompleted;
        }
    }
}