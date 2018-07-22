using BattleCruisers.Buildables.Buildings.Factories;

namespace BattleCruisers.AI.Drones.BuildingMonitors
{
    public interface IFactoryMonitor
    {
        IFactory Factory { get; }
        bool HasFactoryBuiltDesiredNumOfUnits { get; }
    }
}
