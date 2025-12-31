using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.Drones
{
    public class FactoryWastingDronesFilter : IFilter<FactoryMonitor>
    {
        /// <summary>
        /// A factory is wrongly using drones if:
        /// + It has completed building the desired number of units
        /// + AND it is using drones
        /// </summary>
        public bool IsMatch(FactoryMonitor factoryMonitor)
        {
            return
                 factoryMonitor.HasFactoryBuiltDesiredNumOfUnits
                 && factoryMonitor.Factory.DroneConsumer != null
                 && factoryMonitor.Factory.DroneConsumer.State != DroneConsumerState.Idle;
        }
        public bool IsMatch(FactoryMonitor factoryMonitor, VariantPrefab variant)
        {
            // need to implement
            return false;
        }
    }
}