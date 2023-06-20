using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Fog;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPCruiserArgs : IPvPCruiserArgs
    {
        public PvPFaction Faction { get; }
        public IPvPCruiser EnemyCruiser { get; }
        public IPvPUIManager UiManager { get; }
        public IPvPDroneManager DroneManager { get; }
        public IPvPDroneFocuser DroneFocuser { get; }
        public IPvPDroneConsumerProvider DroneConsumerProvider { get; }
        public IPvPFactoryProvider FactoryProvider { get; }
        public IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        public PvPDirection FacingDirection { get; }
        public IPvPRepairManager RepairManager { get; }
        public PvPFogStrength FogStrength { get; }
        public IPvPCruiserHelper Helper { get; }
        public IPvPSlotFilter HighlightableFilter { get; }
        public IPvPBuildProgressCalculator BuildProgressCalculator { get; }
        public IPvPDoubleClickHandler<IPvPBuilding> BuildingDoubleClickHandler { get; }
        public IPvPDoubleClickHandler<IPvPCruiser> CruiserDoubleClickHandler { get; }
        public IPvPManagedDisposable FogOfWarManager { get; }
        public IPvPBroadcastingProperty<bool> HasActiveDrones { get; }

        public PvPCruiserArgs(
            PvPFaction faction,
            IPvPCruiser enemyCruiser,
            IPvPUIManager uiManager,
            IPvPDroneManager droneManager,
            IPvPDroneFocuser droneFocuser,
            IPvPDroneConsumerProvider droneConsumerProvider,
            IPvPFactoryProvider factoryProvider,
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
           PvPDirection facingDirection,
            IPvPRepairManager repairManager,
            PvPFogStrength fogStrength,
            IPvPCruiserHelper helper,
            IPvPSlotFilter highlightableFilter,
            IPvPBuildProgressCalculator buildProgressCalculator,
            IPvPDoubleClickHandler<IPvPBuilding> buildingDoubleClickHandler,
            IPvPDoubleClickHandler<IPvPCruiser> cruiserDoubleClickHandler,
            IPvPManagedDisposable fogOfWarManager,
            IPvPBroadcastingProperty<bool> parentCruiserHasActiveDrones)
        {
            BCUtils.Helper.AssertIsNotNull(
                enemyCruiser,
                uiManager,
                droneManager,
                droneFocuser,
                droneConsumerProvider,
                factoryProvider,
                cruiserSpecificFactories,
                repairManager,
                helper,
                highlightableFilter,
                buildProgressCalculator,
                buildingDoubleClickHandler,
                cruiserDoubleClickHandler,
                fogOfWarManager,
                parentCruiserHasActiveDrones);

            Faction = faction;
            EnemyCruiser = enemyCruiser;
            UiManager = uiManager;
            DroneManager = droneManager;
            DroneFocuser = droneFocuser;
            DroneConsumerProvider = droneConsumerProvider;
            FactoryProvider = factoryProvider;
            CruiserSpecificFactories = cruiserSpecificFactories;
            FacingDirection = facingDirection;
            RepairManager = repairManager;
            FogStrength = fogStrength;
            Helper = helper;
            HighlightableFilter = highlightableFilter;
            BuildProgressCalculator = buildProgressCalculator;
            BuildingDoubleClickHandler = buildingDoubleClickHandler;
            CruiserDoubleClickHandler = cruiserDoubleClickHandler;
            FogOfWarManager = fogOfWarManager;
            HasActiveDrones = parentCruiserHasActiveDrones;
        }


        public PvPCruiserArgs(
            PvPFaction faction,
            IPvPCruiser enemyCruiser,
            // IPvPUIManager uiManager,
            IPvPDroneManager droneManager,
            IPvPDroneFocuser droneFocuser,
            IPvPDroneConsumerProvider droneConsumerProvider,
            IPvPFactoryProvider factoryProvider,
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
            PvPDirection facingDirection,
            IPvPRepairManager repairManager,
            PvPFogStrength fogStrength,
            IPvPCruiserHelper helper,
            IPvPSlotFilter highlightableFilter,
            IPvPBuildProgressCalculator buildProgressCalculator,
            IPvPDoubleClickHandler<IPvPBuilding> buildingDoubleClickHandler,
            IPvPDoubleClickHandler<IPvPCruiser> cruiserDoubleClickHandler,
            IPvPManagedDisposable fogOfWarManager,
            IPvPBroadcastingProperty<bool> parentCruiserHasActiveDrones)
        {
            BCUtils.Helper.AssertIsNotNull(
                enemyCruiser,
                // uiManager,
                droneManager,
                droneFocuser,
                droneConsumerProvider,
                factoryProvider,
                cruiserSpecificFactories,
                repairManager,
                helper,
                highlightableFilter,
                buildProgressCalculator,
                buildingDoubleClickHandler,
                cruiserDoubleClickHandler,
                fogOfWarManager,
                parentCruiserHasActiveDrones);

            Faction = faction;
            EnemyCruiser = enemyCruiser;
            // UiManager = uiManager;
            DroneManager = droneManager;
            DroneFocuser = droneFocuser;
            DroneConsumerProvider = droneConsumerProvider;
            FactoryProvider = factoryProvider;
            CruiserSpecificFactories = cruiserSpecificFactories;
            FacingDirection = facingDirection;
            RepairManager = repairManager;
            FogStrength = fogStrength;
            Helper = helper;
            HighlightableFilter = highlightableFilter;
            BuildProgressCalculator = buildProgressCalculator;
            BuildingDoubleClickHandler = buildingDoubleClickHandler;
            CruiserDoubleClickHandler = cruiserDoubleClickHandler;
            FogOfWarManager = fogOfWarManager;
            HasActiveDrones = parentCruiserHasActiveDrones;
        }
    }
}
