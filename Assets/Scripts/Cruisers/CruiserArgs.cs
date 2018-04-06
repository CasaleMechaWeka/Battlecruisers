using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils;

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
        public ICameraController CameraController { get; private set; }

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
            ICameraController cameraController)
        {
            Helper.AssertIsNotNull(enemyCruiser, uiManager, droneManager, droneConsumerProvider, factoryProvider, repairManager, cameraController);

            Faction = faction;
            EnemyCruiser = enemyCruiser;
            UiManager = uiManager;
            DroneManager = droneManager;
            DroneConsumerProvider = droneConsumerProvider;
            FactoryProvider = factoryProvider;
            FacingDirection = facingDirection;
            RepairManager = repairManager;
            ShouldShowFog = shouldShowFog;
            CameraController = cameraController;
        }
    }
}
