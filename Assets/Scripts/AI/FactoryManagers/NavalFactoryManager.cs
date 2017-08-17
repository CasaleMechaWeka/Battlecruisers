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
    /// Decides which type of boat to build.
    /// 
    /// Builds the most expensive boat we can afford.
    /// 
    /// If we gain enough drones for a more epensive boat, starts building the
    /// more expensive boat when the current boat under construction completes.  
    /// Ie, do not want to build a boat to 90% completion and waste it by 
    /// starting construction on a different boat.
    /// 
    /// If we lose drones and can no longer afford to build the current boat, 
    /// does nothing.  This means no drones are wasted building cheaper boats
    /// and can instead be used to replace lost drone stations and hopefully
    /// continue buildng the current boat once the drone stations are rebuilt.
    /// </summary>
    public class NavalFactoryManager : IFactoryManager
	{
        private readonly ICruiserController _friendlyCruiser;
        private readonly IDroneManager _droneManager;
        private readonly IUnitChooser _unitChooser;

        private IBuildableWrapper<IUnit> _boatToBuild;
        private IFactory _navalFactory;

        public NavalFactoryManager(ICruiserController friendlyCruiser, IUnitChooser unitChooser)
        {
            Helper.AssertIsNotNull(friendlyCruiser, friendlyCruiser.DroneManager, unitChooser);

            _friendlyCruiser = friendlyCruiser;
            _droneManager = _friendlyCruiser.DroneManager;
            _unitChooser = unitChooser;

            _friendlyCruiser.StartedConstruction += _friendlyCruiser_StartedConstruction;
            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;

            _boatToBuild = _unitChooser.ChooseUnit(_droneManager.NumOfDrones);
        }

        private void _friendlyCruiser_StartedConstruction(object sender, StartedConstructionEventArgs e)
        {
            IFactory factory = e.Buildable as IFactory;

            if (factory != null && factory.UnitCategory == UnitCategory.Naval)
            {
                Assert.IsNull(_navalFactory, "Should only be able to build a single naval factory :/");

                _navalFactory = factory;
                _navalFactory.CompletedBuildable += _navalFactory_CompletedBuildable;
                _navalFactory.Destroyed += _navalFactory_Destroyed;
            }
        }

        private void _navalFactory_CompletedBuildable(object sender, EventArgs e)
        {
            _navalFactory.UnitWrapper = _boatToBuild;
            _navalFactory.CompletedBuildingUnit += _navalFactory_CompletedBuildingUnit;
        }

        private void _navalFactory_CompletedBuildingUnit(object sender, CompletedConstructionEventArgs e)
        {
            _navalFactory.UnitWrapper = _boatToBuild;
        }

        private void _navalFactory_Destroyed(object sender, DestroyedEventArgs e)
        {
            _navalFactory.CompletedBuildable -= _navalFactory_CompletedBuildable; 
            _navalFactory.Destroyed -= _navalFactory_Destroyed;
            _navalFactory.CompletedBuildingUnit -= _navalFactory_CompletedBuildingUnit;
            _navalFactory = null;
        }

        private void _droneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            if (e.NewNumOfDrones > e.OldNumOfDrones)
            {
                _boatToBuild = _unitChooser.ChooseUnit(e.NewNumOfDrones);
            }
        }

        public void Dispose()
		{
            _friendlyCruiser.StartedConstruction -= _friendlyCruiser_StartedConstruction;
            _droneManager.DroneNumChanged -= _droneManager_DroneNumChanged;
		}
    }
}
