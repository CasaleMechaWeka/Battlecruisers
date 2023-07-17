using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.BuildingMonitors
{
    public interface IPvPFactoriesMonitor
    {
        IReadOnlyCollection<IPvPFactoryMonitor> CompletedFactories { get; }
    }
}
