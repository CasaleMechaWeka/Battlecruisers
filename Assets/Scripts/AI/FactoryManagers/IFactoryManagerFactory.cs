namespace BattleCruisers.AI.FactoryManagers
{
    public interface IFactoryManagerFactory
    {
        IFactoryManager CreateNavalFactoryManager(ILevelInfo levelInfo);
        IFactoryManager CreateAirfactoryManager(ILevelInfo levelInfo);
	}
}
