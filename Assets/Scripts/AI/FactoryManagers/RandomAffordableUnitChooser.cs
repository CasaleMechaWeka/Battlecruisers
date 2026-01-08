using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.FactoryManagers
{
    /// <summary>
    /// Always picks a random affordable unit. On each completion or drone change, a new random pick is made.
    /// </summary>
    public class RandomAffordableUnitChooser : UnitChooser
    {
        private readonly IList<IBuildableWrapper<IUnit>> _units;
        private readonly DroneManager _droneManager;

        public RandomAffordableUnitChooser(IList<IBuildableWrapper<IUnit>> units, DroneManager droneManager)
        {
            Helper.AssertIsNotNull(units, droneManager);
            Assert.IsTrue(units.Count != 0);

            _units = units;
            _droneManager = droneManager;

            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
            ChooseUnit();
        }

        private void _droneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            ChooseUnit();
        }

        private void ChooseUnit()
        {
            IList<IBuildableWrapper<IUnit>> affordableUnits =
                _units.Where(wrapper => wrapper.Buildable.NumOfDronesRequired <= _droneManager.NumOfDrones).ToList();

            if (affordableUnits.Count == 0)
            {
                ChosenUnit = null;
                return;
            }

            int idx = UnityEngine.Random.Range(0, affordableUnits.Count);
            ChosenUnit = affordableUnits[idx];
        }

        public override void OnUnitBuilt()
        {
            ChooseUnit();
        }

        public override void DisposeManagedState()
        {
            _droneManager.DroneNumChanged -= _droneManager_DroneNumChanged;
        }
    }
}


