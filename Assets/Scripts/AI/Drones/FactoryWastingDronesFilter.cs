using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.Drones
{
    public class FactoryWastingDronesFilter : IFilter<IFactoryMonitor>
    {
        /// <summary>
        /// A factory is wrongly using drones if:
        /// + It has completed building the desired number of units
        /// + AND it is using drones
        /// </summary>
        public bool IsMatch(IFactoryMonitor factoryMonitor)
        {
            return
                 factoryMonitor.HasFactoryBuiltDesiredNumOfUnits
                 && factoryMonitor.Factory.DroneConsumer != null
                 && factoryMonitor.Factory.DroneConsumer.State != DroneConsumerState.Idle;
        }
        public bool IsMatch(IFactoryMonitor factoryMonitor, VariantPrefab variant)
        {
            // need to implement
            return false;
        }
    }
}