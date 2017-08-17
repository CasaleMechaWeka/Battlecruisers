using BattleCruisers.Cruisers;
using BattleCruisers.Drones;

namespace BattleCruisers.AI.FactoryManagers
{
    public interface IFactoryManagerFactory
    {
        IFactoryManager CreateNavalFactoryManager(int levelNum, ICruiserController friendlyCruiser, IDroneManager droneManager);
    }
}
