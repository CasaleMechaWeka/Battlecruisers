using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using System.Linq;

namespace BattleCruisers.AI.Drones
{
    public class AffordableInProgressNonFocusedProvider : IBuildingProvider
    {
        private readonly IDroneManager _droneManager;
        private readonly IInProgressBuildingMonitor _buildingMonitor;

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

        public AffordableInProgressNonFocusedProvider(IDroneManager droneManager, IInProgressBuildingMonitor buildingMonitor)
        {
            Helper.AssertIsNotNull(droneManager, buildingMonitor);

            _droneManager = droneManager;
            _buildingMonitor = buildingMonitor;
        }
    }
}
