using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.BuildingMonitors
{
    public interface IPvPFactoryMonitor
    {
        IPvPFactory Factory { get; }
        bool HasFactoryBuiltDesiredNumOfUnits { get; }
    }
}
