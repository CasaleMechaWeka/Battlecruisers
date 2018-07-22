using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using System.Linq;

namespace BattleCruisers.AI.Drones
{
    // FELIX  NEXT Test
    public class AffordableInProgressNonAffordableProvider : IBuildingProvider
    {
        private readonly IDroneManager _droneManager;
        private readonly IInProgressBuildingMonitor _buildingMonitor;

        public IBuildable Building
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

        public AffordableInProgressNonAffordableProvider(IDroneManager droneManager, IInProgressBuildingMonitor buildingMonitor)
        {
            Helper.AssertIsNotNull(droneManager, buildingMonitor);

            _droneManager = droneManager;
            _buildingMonitor = buildingMonitor;
        }
    }
}
