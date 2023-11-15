using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones
{
    public class PvPDroneConsumerFocusHelper : IPvPDroneConsumerFocusHelper
    {
        private readonly IPvPDroneManager _droneManager;
        private readonly IPvPFactoryAnalyzer _factoryAnalyzer;
        private readonly IPvPBuildingProvider _affordableInProgressNonFocusedBuildingProvider;

        public PvPDroneConsumerFocusHelper(IPvPDroneManager droneManager, IPvPFactoryAnalyzer factoryAnalyzer, IPvPBuildingProvider affordableInProgressNonFocusedBuildingProvider)
        {
            PvPHelper.AssertIsNotNull(droneManager, factoryAnalyzer, affordableInProgressNonFocusedBuildingProvider);

            _droneManager = droneManager;
            _factoryAnalyzer = factoryAnalyzer;
            _affordableInProgressNonFocusedBuildingProvider = affordableInProgressNonFocusedBuildingProvider;
        }

        public void FocusOnNonFactoryDroneConsumer(bool forceInProgressBuildingToFocused)
        {
           // Logging.LogMethod(Tags.DRONE_CONUMSER_FOCUS_MANAGER);

            if (!_factoryAnalyzer.AreAnyFactoriesWronglyUsingDrones)
            {
                // No factories wrongly using drones, no need to reassign drones
                return;
            }

            IPvPBuildable affordableBuilding = _affordableInProgressNonFocusedBuildingProvider.Building;
            if (affordableBuilding == null)
            {
                // No affordable buildings, so no buildings to assign wrongly used drones to
                return;
            }

            //  Logging.Log(Tags.DRONE_CONUMSER_FOCUS_MANAGER, "Going to focus on: " + affordableBuilding);
            IPvPDroneConsumer affordableDroneConsumer = affordableBuilding.DroneConsumer;

            // Try to upgrade: Idle => Active
            if (affordableDroneConsumer.State == PvPDroneConsumerState.Idle)
            {
                _droneManager.ToggleDroneConsumerFocus(affordableDroneConsumer);
            }

            // Try to upgrade: Active => Focused
            if (affordableDroneConsumer.State == PvPDroneConsumerState.Active
                && forceInProgressBuildingToFocused)
            {
                _droneManager.ToggleDroneConsumerFocus(affordableDroneConsumer);
            }
        }
    }
}
