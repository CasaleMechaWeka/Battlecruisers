using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.Drones
{
    public class DroneConsumerFocusHelper : IDroneConsumerFocusHelper
    {
        private readonly IDroneManager _droneManager;
        private readonly IFactoryAnalyzer _factoryAnalyzer;
        private readonly IBuildingProvider _affordableInProgressNonFocusedBuildingProvider;

        public DroneConsumerFocusHelper(IDroneManager droneManager, IFactoryAnalyzer factoryAnalyzer, IBuildingProvider affordableInProgressNonFocusedBuildingProvider)
        {
            Helper.AssertIsNotNull(droneManager, factoryAnalyzer, affordableInProgressNonFocusedBuildingProvider);

            _droneManager = droneManager;
            _factoryAnalyzer = factoryAnalyzer;
            _affordableInProgressNonFocusedBuildingProvider = affordableInProgressNonFocusedBuildingProvider;
        }

        public void FocusOnNonFactoryDroneConsumer(bool forceInProgressBuildingToFocused)
        {
            Logging.LogMethod(Tags.DRONE_CONUMSER_FOCUS_MANAGER);

            if (!_factoryAnalyzer.AreAnyFactoriesWronglyUsingDrones)
            {
                // No factories wrongly using drones, no need to reassign drones
                return;
            }

            IBuildable affordableBuilding = _affordableInProgressNonFocusedBuildingProvider.Building;
            if (affordableBuilding == null)
            {
                // No affordable buildings, so no buildings to assign wrongly used drones to
                return;
            }

            Logging.Log(Tags.DRONE_CONUMSER_FOCUS_MANAGER, "Going to focus on: " + affordableBuilding);
            IDroneConsumer affordableDroneConsumer = affordableBuilding.DroneConsumer;

            // Try to upgrade: Idle => Active
            if (affordableDroneConsumer.State == DroneConsumerState.Idle)
            {
                _droneManager.ToggleDroneConsumerFocus(affordableDroneConsumer);
            }

            // Try to upgrade: Active => Focused
            if (affordableDroneConsumer.State == DroneConsumerState.Active
                && forceInProgressBuildingToFocused)
            {
                _droneManager.ToggleDroneConsumerFocus(affordableDroneConsumer);
            }
        }
    }
}
