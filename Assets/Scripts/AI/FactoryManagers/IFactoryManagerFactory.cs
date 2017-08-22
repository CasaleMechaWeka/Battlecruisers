using BattleCruisers.Cruisers;

namespace BattleCruisers.AI.FactoryManagers
{
    public interface IFactoryManagerFactory
    {
        IFactoryManager CreateNavalFactoryManager(int levelNum, ICruiserController friendlyCruiser);
    }
}
