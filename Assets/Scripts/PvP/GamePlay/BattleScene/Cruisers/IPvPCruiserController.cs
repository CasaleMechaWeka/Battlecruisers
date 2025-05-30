using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Targets.TargetTrackers;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public interface IPvPCruiserController
    {
        bool IsAlive { get; }

        PvPSlotAccessor SlotAccessor { get; }
        PvPSlotHighlighter SlotHighlighter { get; }
        ISlotNumProvider SlotNumProvider { get; }
        DroneManager DroneManager { get; }
        IPvPDroneFocuser DroneFocuser { get; }
        IPvPCruiserBuildingMonitor BuildingMonitor { get; }
        IPvPCruiserUnitMonitor UnitMonitor { get; }
        IPopulationLimitMonitor PopulationLimitMonitor { get; }
        IUnitTargets UnitTargets { get; }
        TargetTracker BlockedShipsTracker { get; }

        event EventHandler<PvPBuildingStartedEventArgs> BuildingStarted;

        IPvPBuilding ConstructBuilding(IPvPBuildableWrapper<IPvPBuilding> buildingPrefab, IPvPSlot slot);
    }
}

