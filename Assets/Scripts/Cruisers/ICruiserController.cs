using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using System;

namespace BattleCruisers.Cruisers
{
	public interface ICruiserController
	{
        bool IsAlive { get; }
        ISlotAccessor SlotAccessor { get; }
        ISlotHighlighter SlotHighlighter { get; }
        ISlotNumProvider SlotNumProvider { get; }
		IDroneManager DroneManager { get; }
        IDroneFocuser DroneFocuser { get; }
        ICruiserBuildingMonitor BuildingMonitor { get; }
        ICruiserUnitMonitor UnitMonitor { get; }

        event EventHandler<BuildingStartedEventArgs> BuildingStarted;

        IBuilding ConstructBuilding(IBuildableWrapper<IBuilding> buildingPrefab, ISlot slot);
	}
}
