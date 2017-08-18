using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.FactoryManagers
{
    /// <summary>
    /// Monitors all factories for a unit category (air/naval).
    /// 
    /// This manager asks the injected IUnitChooser which unit a factory should
    /// build when the number of drones aivailable to the cruiser change.
    /// 
    /// FELIX  Never cache unit to build.  ALWAYS query unit chooser!
    /// 
    /// This manager sets the unit to build for a factory when that factory:
    /// a) Completes building (itself)
    /// b) Completes building a unit
    /// </summary>
    public class FactoryManager : IFactoryManager
    {
        private readonly UnitCategory _factoryUnitCategory;
        private readonly ICruiserController _friendlyCruiser;
        private readonly IDroneManager _droneManager;
        private readonly IUnitChooser _unitChooser;

        private IBuildableWrapper<IUnit> _unitToBuild;

        public FactoryManager(UnitCategory factoryUnitCategory, ICruiserController friendlyCruiser, IUnitChooser unitChooser)
        {
            Helper.AssertIsNotNull(friendlyCruiser, friendlyCruiser.DroneManager, unitChooser);

            _factoryUnitCategory = factoryUnitCategory;
            _friendlyCruiser = friendlyCruiser;
            _droneManager = _friendlyCruiser.DroneManager;
            _unitChooser = unitChooser;

            _friendlyCruiser.StartedConstruction += _friendlyCruiser_StartedConstruction;
            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;

            _unitToBuild = _unitChooser.ChooseUnit(_droneManager.NumOfDrones);
        }

        private void _friendlyCruiser_StartedConstruction(object sender, StartedConstructionEventArgs e)
        {
            IFactory factory = e.Buildable as IFactory;

            if (factory != null && factory.UnitCategory == _factoryUnitCategory)
            {
                factory.CompletedBuildable += Factory_CompletedBuildable;
                factory.Destroyed += Factory_Destroyed;
            }
        }

        private void Factory_CompletedBuildable(object sender, EventArgs e)
        {
            IFactory factory = sender as IFactory;
            Assert.IsNotNull(factory);

            factory.UnitWrapper = _unitToBuild;
            factory.CompletedBuildingUnit += Factory_CompletedBuildingUnit;
        }

        private void Factory_CompletedBuildingUnit(object sender, CompletedConstructionEventArgs e)
        {
			IFactory factory = sender as IFactory;
			Assert.IsNotNull(factory);

            factory.UnitWrapper = _unitToBuild;
        }

        private void Factory_Destroyed(object sender, DestroyedEventArgs e)
        {
			IFactory factory = sender as IFactory;
			Assert.IsNotNull(factory);

            factory.CompletedBuildable -= Factory_CompletedBuildable; 
            factory.Destroyed -= Factory_Destroyed;
            factory.CompletedBuildingUnit -= Factory_CompletedBuildingUnit;
        }

        private void _droneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            // FELIX  Remove check, always choose!
            // FELIX  Can remove old num of drones from event again :(
            if (e.NewNumOfDrones > e.OldNumOfDrones)
            {
                _unitToBuild = _unitChooser.ChooseUnit(e.NewNumOfDrones);
            }
        }

        public void Dispose()
        {
            _friendlyCruiser.StartedConstruction -= _friendlyCruiser_StartedConstruction;
            _droneManager.DroneNumChanged -= _droneManager_DroneNumChanged;
        }
    }
}
