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
        public Faction Faction { get; private set; }
        public ICruiser EnemyCruiser { get; private set; }
        public IUIManager UiManager { get; private set; }
        public IDroneManager DroneManager { get; private set; }
        public IDroneConsumerProvider DroneConsumerProvider { get; private set; }
        public IFactoryProvider FactoryProvider { get; private set; }
        public Direction FacingDirection { get; private set; }
        public RepairManager RepairManager { get; private set; }
        public bool ShouldShowFog { get; private set; }
        public ICruiserHelper Helper { get; private set; }
        public ISlotFilter HighlightableFilter { get; private set; }
        public IBuildProgressCalculator BuildProgressCalculator { get; private set; }
        public IDoubleClickHandler<IBuilding> BuildingDoubleClickHandler { get; private set; }
        public IDoubleClickHandler<ICruiser> CruiserDoubleClickHandler { get; private set; }
        public FogOfWarManager FogOfWarManager { get; private set; }

        public CruiserArgs(
            Faction faction, 
            ICruiser enemyCruiser, 
            IUIManager uiManager, 
            IDroneManager droneManager, 
            IDroneConsumerProvider droneConsumerProvider,
            IFactoryProvider factoryProvider, 
            Direction facingDirection, 
            RepairManager repairManager, 
            bool shouldShowFog,
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
            DroneConsumerProvider = droneConsumerProvider;
            FactoryProvider = factoryProvider;
            FacingDirection = facingDirection;
            RepairManager = repairManager;
            ShouldShowFog = shouldShowFog;
            Helper = helper;
            HighlightableFilter = highlightableFilter;
            BuildProgressCalculator = buildProgressCalculator;
            BuildingDoubleClickHandler = buildingDoubleClickHandler;
            CruiserDoubleClickHandler = cruiserDoubleClickHandler;
            FogOfWarManager = fogOfWarManager;
        }
    }
}
