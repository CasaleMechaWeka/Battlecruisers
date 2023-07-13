using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.FactoryManagers
{
    public interface IPvPFactoryManagerFactory
    {
        IPvPFactoryManager CreateNavalFactoryManager(PvPCruiser aiCruiser);
        IPvPFactoryManager CreateAirfactoryManager(PvPCruiser aiCruiser);
    }
}
