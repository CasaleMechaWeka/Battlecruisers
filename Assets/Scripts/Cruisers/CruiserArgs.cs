using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Drones;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers
{
    public class CruiserArgs : ICruiserArgs
    {
        public Faction Faction { get; private set; }
        public ICruiser EnemyCruiser { get; private set; }
        public HealthBarController HealthBarController { get; private set; }
        public IUIManager UiManager { get; private set; }
        public IDroneManager DroneManager { get; private set; }
        public IDroneConsumerProvider DroneConsumerProvider { get; private set; }
        public IFactoryProvider FactoryProvider { get; private set; }
        public Direction FacingDirection { get; private set; }
        public RepairManager RepairManager { get; private set; }
        public bool ShouldShowFog { get; private set; }

        public CruiserArgs(Faction faction, ICruiser enemyCruiser, HealthBarController healthBarController,
            IUIManager uiManager, IDroneManager droneManager, IDroneConsumerProvider droneConsumerProvider,
            IFactoryProvider factoryProvider, Direction facingDirection, RepairManager repairManager, bool shouldShowFog)
        {
            Faction = faction;
            EnemyCruiser = enemyCruiser;
            HealthBarController = healthBarController;
            UiManager = uiManager;
            DroneManager = droneManager;
            DroneConsumerProvider = droneConsumerProvider;
            FactoryProvider = factoryProvider;
            FacingDirection = facingDirection;
            RepairManager = repairManager;
            ShouldShowFog = shouldShowFog;
        }
    }
}
