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
using BattleCruisers.Utils.Factories;
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
        public Direction FacingDirection { get; }
        public IRepairManager RepairManager { get; }
        public FogStrength FogStrength { get; }
        public ICruiserHelper Helper { get; }
        public ISlotFilter HighlightableFilter { get; }
        public IBuildProgressCalculator BuildProgressCalculator { get; }
        public IDoubleClickHandler<IBuilding> BuildingDoubleClickHandler { get; }
        public IDoubleClickHandler<ICruiser> CruiserDoubleClickHandler { get; }
        public FogOfWarManager FogOfWarManager { get; }

        public CruiserArgs(
            Faction faction, 
            ICruiser enemyCruiser, 
            IUIManager uiManager, 
            IDroneManager droneManager,
            IDroneFocuser droneFocuser,
            IDroneConsumerProvider droneConsumerProvider,
            IFactoryProvider factoryProvider, 
            Direction facingDirection, 
            IRepairManager repairManager, 
            FogStrength fogStrength,
            ICruiserHelper helper,
            ISlotFilter highlightableFilter,
            IBuildProgressCalculator buildProgressCalculator,
            IDoubleClickHandler<IBuilding> buildingDoubleClickHandler,
            IDoubleClickHandler<ICruiser> cruiserDoubleClickHandler,
            FogOfWarManager fogOfWarManager)
        {
            BCUtils.Helper.AssertIsNotNull(
                enemyCruiser, 
                uiManager, 
                droneManager, 
                droneFocuser,
                droneConsumerProvider, 
                factoryProvider, 
                repairManager, 
                helper, 
                highlightableFilter, 
                buildProgressCalculator,
                buildingDoubleClickHandler,
                cruiserDoubleClickHandler,
                fogOfWarManager);

            Faction = faction;
            EnemyCruiser = enemyCruiser;
            UiManager = uiManager;
            DroneManager = droneManager;
            DroneFocuser = droneFocuser;
            DroneConsumerProvider = droneConsumerProvider;
            FactoryProvider = factoryProvider;
            FacingDirection = facingDirection;
            RepairManager = repairManager;
            FogStrength = fogStrength;
            Helper = helper;
            HighlightableFilter = highlightableFilter;
            BuildProgressCalculator = buildProgressCalculator;
            BuildingDoubleClickHandler = buildingDoubleClickHandler;
            CruiserDoubleClickHandler = cruiserDoubleClickHandler;
            FogOfWarManager = fogOfWarManager;
        }
    }
}
