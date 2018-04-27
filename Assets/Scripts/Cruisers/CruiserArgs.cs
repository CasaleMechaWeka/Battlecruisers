using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.UI.BattleScene.Manager;
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
        public BCUtils.IFactoryProvider FactoryProvider { get; private set; }
        public Direction FacingDirection { get; private set; }
        public RepairManager RepairManager { get; private set; }
        public bool ShouldShowFog { get; private set; }
        public ICruiserHelper Helper { get; private set; }

        public CruiserArgs(
            Faction faction, 
            ICruiser enemyCruiser, 
            IUIManager uiManager, 
            IDroneManager droneManager, 
            IDroneConsumerProvider droneConsumerProvider,
            BCUtils.IFactoryProvider factoryProvider, 
            Direction facingDirection, 
            RepairManager repairManager, 
            bool shouldShowFog,
            ICruiserHelper helper)
        {
            BCUtils.Helper.AssertIsNotNull(enemyCruiser, uiManager, droneManager, droneConsumerProvider, factoryProvider, repairManager, helper);

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
        }
    }
}
