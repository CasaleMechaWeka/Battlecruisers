using System;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public interface IFactory : IBuilding
    {
		UnitCategory UnitCategory { get; }
        int NumOfDrones { get; }
        IBuildableWrapper<IUnit> UnitWrapper { get; set; }

        event EventHandler<StartedConstructionEventArgs> StartedBuildingUnit;
		event EventHandler<CompletedConstructionEventArgs> CompletedBuildingUnit;
		event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;
	}
}
