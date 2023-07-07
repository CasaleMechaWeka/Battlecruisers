using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.FactoryManagers
{
    public interface IPvPFactoryManagerFactory
    {
        IPvPFactoryManager CreateNavalFactoryManager(IPvPCruiserController aiCruiser);
        IPvPFactoryManager CreateAirfactoryManager(IPvPCruiserController aiCruiser);
    }
}
