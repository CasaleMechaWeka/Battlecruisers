using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.Drones
{
    public class DroneConsumerFocusHelper : IDroneConsumerFocusHelper
    {
        private readonly IDroneManager _droneManager;
        private readonly IFactoriesMonitor _factoriesMonitor;
        private readonly IBuildingMonitor _buildingMonitor;

        public DroneConsumerFocusHelper(IDroneManager droneManager, IFactoriesMonitor factoriesMonitor, IBuildingMonitor buildingMonitor)
        {
            Helper.AssertIsNotNull(droneManager, factoriesMonitor, buildingMonitor);

            _droneManager = droneManager;
            _factoriesMonitor = factoriesMonitor;
            _buildingMonitor = buildingMonitor;
        }

        public void FocusOnNonFactoryDroneConsumer(bool forceInProgressBuildingToFocused)
        {
            Logging.Log(Tags.DRONE_CONUMSER_FOCUS_MANAGER, "FocusOnNonFactoryDroneConsumer()");

            if (!_factoriesMonitor.AreAnyFactoriesWronglyUsingDrones)
            {
                // No factories wrongly using drones, no need to reassign drones
                return;
            }

            IBuildable affordableBuilding = _buildingMonitor.GetNonFocusedAffordableBuilding();
            if (affordableBuilding == null)
            {
                // No affordable buildings, so no buildings to assign wrongly used drones to
                return;
            }

            Logging.Log(Tags.DRONE_CONUMSER_FOCUS_MANAGER, "FocusOnNonFactoryDroneConsumer()  Going to focus on: " + affordableBuilding);
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
