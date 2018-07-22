using System;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public interface IFactory : IBuilding
    {
		UnitCategory UnitCategory { get; }
        int NumOfDrones { get; }
        IBuildableWrapper<IUnit> UnitWrapper { get; set; }

        event EventHandler<StartedUnitConstructionEventArgs> StartedBuildingUnit;
		event EventHandler<CompletedUnitConstructionEventArgs> CompletedBuildingUnit;
	}
}
