using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Properties;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Cruisers
{
    public class CruiserArgs : ICruiserArgs
    {
        public Faction Faction { get; }
        public ICruiser EnemyCruiser { get; }
        public IUIManager UiManager { get; }
        public IDroneManager DroneManager { get; }
        public IDroneFocuser DroneFocuser { get; }
        public IDroneConsumerProvider DroneConsumerProvider { get; }
        public IFactoryProvider FactoryProvider { get; }
        public ICruiserSpecificFactories CruiserSpecificFactories { get; }
        public Direction FacingDirection { get; }
        public IRepairManager RepairManager { get; }
        public FogStrength FogStrength { get; }
        public ICruiserHelper Helper { get; }
        public ISlotFilter HighlightableFilter { get; }
        public IBuildProgressCalculator BuildProgressCalculator { get; }
        public IDoubleClickHandler<IBuilding> BuildingDoubleClickHandler { get; }
        public IDoubleClickHandler<ICruiser> CruiserDoubleClickHandler { get; }
        public IManagedDisposable FogOfWarManager { get; }
        public IBroadcastingProperty<bool> HasActiveDrones { get; }

        public CruiserArgs(
            Faction faction, 
            ICruiser enemyCruiser, 
            IUIManager uiManager, 
            IDroneManager droneManager,
            IDroneFocuser droneFocuser,
            IDroneConsumerProvider droneConsumerProvider,
            IFactoryProvider factoryProvider,
            ICruiserSpecificFactories cruiserSpecificFactories,
            Direction facingDirection, 
            IRepairManager repairManager, 
            FogStrength fogStrength,
            ICruiserHelper helper,
            ISlotFilter highlightableFilter,
            IBuildProgressCalculator buildProgressCalculator,
            IDoubleClickHandler<IBuilding> buildingDoubleClickHandler,
            IDoubleClickHandler<ICruiser> cruiserDoubleClickHandler,
            IManagedDisposable fogOfWarManager,
            IBroadcastingProperty<bool> parentCruiserHasActiveDrones)
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
    }
}
