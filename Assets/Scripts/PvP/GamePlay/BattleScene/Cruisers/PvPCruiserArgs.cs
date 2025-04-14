using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Fog;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPCruiserArgs
    {
        public Faction Faction { get; }
        public IPvPCruiser EnemyCruiser { get; }
        public IPvPUIManager UiManager { get; }
        public DroneManager DroneManager { get; }
        public IPvPDroneFocuser DroneFocuser { get; }
        public IDroneConsumerProvider DroneConsumerProvider { get; }
        public IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        public Direction FacingDirection { get; }
        public IRepairManager RepairManager { get; }
        public PvPFogStrength FogStrength { get; }
        public IPvPCruiserHelper Helper { get; }
        public IFilter<IPvPSlot> HighlightableFilter { get; }
        public IPvPBuildProgressCalculator BuildProgressCalculator { get; }
        public IDoubleClickHandler<IPvPBuilding> BuildingDoubleClickHandler { get; }
        public IDoubleClickHandler<IPvPCruiser> CruiserDoubleClickHandler { get; }
        public IManagedDisposable FogOfWarManager { get; }
        public IBroadcastingProperty<bool> HasActiveDrones { get; }

        public PvPCruiserArgs(
            Faction faction,
            IPvPCruiser enemyCruiser,
            IPvPUIManager uiManager,
            DroneManager droneManager,
            IPvPDroneFocuser droneFocuser,
            IDroneConsumerProvider droneConsumerProvider,
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
           Direction facingDirection,
            IRepairManager repairManager,
            PvPFogStrength fogStrength,
            IPvPCruiserHelper helper,
            IFilter<IPvPSlot> highlightableFilter,
            IPvPBuildProgressCalculator buildProgressCalculator,
            IDoubleClickHandler<IPvPBuilding> buildingDoubleClickHandler,
            IDoubleClickHandler<IPvPCruiser> cruiserDoubleClickHandler,
            IManagedDisposable fogOfWarManager,
            IBroadcastingProperty<bool> parentCruiserHasActiveDrones)
        {
            BCUtils.Helper.AssertIsNotNull(
                enemyCruiser,
                uiManager,
                droneManager,
                droneFocuser,
                droneConsumerProvider,
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
            Faction faction,
            IPvPCruiser enemyCruiser,
            // IPvPUIManager uiManager,
            DroneManager droneManager,
            IPvPDroneFocuser droneFocuser,
            IDroneConsumerProvider droneConsumerProvider,
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
            Direction facingDirection,
            IRepairManager repairManager,
            PvPFogStrength fogStrength,
            IPvPCruiserHelper helper,
            IFilter<IPvPSlot> highlightableFilter,
            IPvPBuildProgressCalculator buildProgressCalculator,
            IDoubleClickHandler<IPvPBuilding> buildingDoubleClickHandler,
            IDoubleClickHandler<IPvPCruiser> cruiserDoubleClickHandler,
            IManagedDisposable fogOfWarManager,
            IBroadcastingProperty<bool> parentCruiserHasActiveDrones)
        {
            BCUtils.Helper.AssertIsNotNull(
                enemyCruiser,
                // uiManager,
                droneManager,
                droneFocuser,
                droneConsumerProvider,
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
