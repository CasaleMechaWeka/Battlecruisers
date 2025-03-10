using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.BuildingMonitors;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones
{
    public class PvPFactoryWastingDronesFilter : IFilter<IPvPFactoryMonitor>
    {
        /// <summary>
        /// A factory is wrongly using drones if:
        /// + It has completed building the desired number of units
        /// + AND it is using drones
        /// </summary>
        public bool IsMatch(IPvPFactoryMonitor factoryMonitor)
        {
            return
                 factoryMonitor.HasFactoryBuiltDesiredNumOfUnits
                 && factoryMonitor.Factory.DroneConsumer != null
                 && factoryMonitor.Factory.DroneConsumer.State != DroneConsumerState.Idle;
        }

        public bool IsMatch(IPvPFactoryMonitor factoryMonitor, VariantPrefab variant)
        {
            // need to implement
            return false;
        }
    }
}