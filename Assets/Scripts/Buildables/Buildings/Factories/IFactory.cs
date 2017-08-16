using System;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Drones;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public interface IFactory : IBuilding
    {
		UnitCategory UnitCategory { get; }
        int NumOfDrones { get; }
        IBuildableWrapper<IUnit> UnitWrapper { set; }

		event EventHandler<DroneNumChangedEventArgs> DroneNumChanged;
	}
}