using BattleCruisers.Cruisers;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.FactoryManagers
{
    public interface IFactoryManagerFactory
    {
        IManagedDisposable CreateNavalFactoryManager(ICruiserController aiCruiser);
        IManagedDisposable CreateAirfactoryManager(ICruiserController aiCruiser);
    }
}
