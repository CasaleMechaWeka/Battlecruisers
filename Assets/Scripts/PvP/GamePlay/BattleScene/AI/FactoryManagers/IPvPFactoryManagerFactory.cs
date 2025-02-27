using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.FactoryManagers
{
    public interface IManagedDisposableFactory
    {
        IManagedDisposable CreateNavalFactoryManager(PvPCruiser aiCruiser);
        IManagedDisposable CreateAirfactoryManager(PvPCruiser aiCruiser);
    }
}
