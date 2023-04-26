using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using System;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public interface IPvPCruiserController
    {
        bool IsAlive { get; }
        IPvPSlotAccessor SlotAccessor { get; }
        IPvPSlotHighlighter SlotHighlighter { get; }
        IPvPSlotNumProvider SlotNumProvider { get; }
        IPvPDroneManager DroneManager { get; }
        IPvPDroneFocuser DroneFocuser { get; }
        IPvPCruiserBuildingMonitor BuildingMonitor { get; }
        IPvPCruiserUnitMonitor UnitMonitor { get; }
        IPvPPopulationLimitMonitor PopulationLimitMonitor { get; }
        IPvPUnitTargets UnitTargets { get; }
        IPvPTargetTracker BlockedShipsTracker { get; }

        event EventHandler<PvPBuildingStartedEventArgs> BuildingStarted;

        IPvPBuilding ConstructBuilding(IPvPBuildableWrapper<IPvPBuilding> buildingPrefab, IPvPSlot slot);
    }
}

