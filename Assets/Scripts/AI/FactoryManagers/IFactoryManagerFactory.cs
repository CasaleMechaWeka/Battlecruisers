using BattleCruisers.Cruisers;

namespace BattleCruisers.AI.FactoryManagers
{
    public interface IFactoryManagerFactory
    {
        IFactoryManager CreateNavalFactoryManager(ICruiserController aiCruiser);
        IFactoryManager CreateAirfactoryManager(ICruiserController aiCruiser);
	}
}
