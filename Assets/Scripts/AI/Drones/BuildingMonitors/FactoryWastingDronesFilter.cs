using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.Drones.BuildingMonitors
{
    public class FactoryWastingDronesFilter : IFilter<IFactoryMonitor>
    {
        public bool IsMatch(IFactoryMonitor factoryMonitor)
        {
            return
                 factoryMonitor.HasFactoryBuiltDesiredNumOfUnits
                 && factoryMonitor.Factory.DroneConsumer != null
                 && factoryMonitor.Factory.DroneConsumer.State != DroneConsumerState.Idle;
        }
    }
}