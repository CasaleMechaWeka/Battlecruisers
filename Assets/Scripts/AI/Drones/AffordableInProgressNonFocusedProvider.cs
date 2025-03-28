using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using System.Linq;

namespace BattleCruisers.AI.Drones
{
    public class AffordableInProgressNonFocusedProvider
    {
        private readonly IDroneManager _droneManager;
        private readonly InProgressBuildingMonitor _buildingMonitor;

        public IBuilding Building
        {
            get
            {
                return
                    _buildingMonitor
                        .InProgressBuildings
                        .FirstOrDefault(building =>
                            building.DroneConsumer.State != DroneConsumerState.Focused
                            && building.DroneConsumer.NumOfDronesRequired <= _droneManager.NumOfDrones);
            }
        }

        public AffordableInProgressNonFocusedProvider(IDroneManager droneManager, InProgressBuildingMonitor buildingMonitor)
        {
            Helper.AssertIsNotNull(droneManager, buildingMonitor);

            _droneManager = droneManager;
            _buildingMonitor = buildingMonitor;
        }
    }
}
