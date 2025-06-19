using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Targets.TargetTrackers;
using System;

namespace BattleCruisers.Cruisers
{
    public interface ICruiserController
    {
        bool IsAlive { get; }
        SlotAccessor SlotAccessor { get; }
        SlotHighlighter SlotHighlighter { get; }
        ISlotNumProvider SlotNumProvider { get; }
        DroneManager DroneManager { get; }
        IDroneFocuser DroneFocuser { get; }
        ICruiserBuildingMonitor BuildingMonitor { get; }
        ICruiserUnitMonitor UnitMonitor { get; }
        IPopulationLimitMonitor PopulationLimitMonitor { get; }
        IUnitTargets UnitTargets { get; }
        TargetTracker BlockedShipsTracker { get; }

        event EventHandler<BuildingStartedEventArgs> BuildingStarted;

        IBuilding ConstructBuilding(IBuildableWrapper<IBuilding> buildingPrefab, Slot slot);
    }
}
