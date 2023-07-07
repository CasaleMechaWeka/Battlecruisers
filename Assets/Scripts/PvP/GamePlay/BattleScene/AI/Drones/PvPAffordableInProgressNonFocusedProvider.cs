using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.BuildingMonitors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Linq;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones
{
    public class PvPAffordableInProgressNonFocusedProvider : IPvPBuildingProvider
    {
        private readonly IPvPDroneManager _droneManager;
        private readonly IPvPInProgressBuildingMonitor _buildingMonitor;

        public IPvPBuilding Building
        {
            get
            {
                return
                    _buildingMonitor
                        .InProgressBuildings
                        .FirstOrDefault(building =>
                            building.DroneConsumer.State != PvPDroneConsumerState.Focused
                            && building.DroneConsumer.NumOfDronesRequired <= _droneManager.NumOfDrones);
            }
        }

        public PvPAffordableInProgressNonFocusedProvider(IPvPDroneManager droneManager, IPvPInProgressBuildingMonitor buildingMonitor)
        {
            PvPHelper.AssertIsNotNull(droneManager, buildingMonitor);

            _droneManager = droneManager;
            _buildingMonitor = buildingMonitor;
        }
    }
}
