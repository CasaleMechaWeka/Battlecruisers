using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Utils;

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
        // To know when a naval factory is under construction
        private readonly ICruiserController _friendlyCruiser;
        // To know how many drones we have available
        private readonly IDroneManager _droneManager;

        private IList<IBuildableWrapper<IUnit>> _boatPrefabs;
        private IFactory _navalFactory;

        public NavalFactoryManager(ICruiserController friendlyCruiser, IDroneManager droneManager)
        {
            Helper.AssertIsNotNull(friendlyCruiser, droneManager);

            _friendlyCruiser = friendlyCruiser;
            _droneManager = droneManager;

            _friendlyCruiser.StartedConstruction += _friendlyCruiser_StartedConstruction;
            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
        }

        void _friendlyCruiser_StartedConstruction(object sender, StartedConstructionEventArgs e)
        {

        }

        void _droneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {

        }

        public void Dispose()
		{
            _friendlyCruiser.StartedConstruction -= _friendlyCruiser_StartedConstruction;
            _droneManager.DroneNumChanged -= _droneManager_DroneNumChanged;
		}
    }
}