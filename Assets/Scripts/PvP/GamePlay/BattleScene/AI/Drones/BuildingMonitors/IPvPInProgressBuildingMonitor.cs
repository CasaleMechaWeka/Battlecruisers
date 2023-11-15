using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.BuildingMonitors
{
    public interface IPvPInProgressBuildingMonitor
    {
        ReadOnlyCollection<IPvPBuilding> InProgressBuildings { get; }
    }
}
