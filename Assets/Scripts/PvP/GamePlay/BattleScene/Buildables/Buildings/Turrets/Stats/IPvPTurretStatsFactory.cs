using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public interface IPvPTurretStatsFactory
    {
        IPvPTurretStats CreateBoostedTurretStats(
            IPvPTurretStats baseTurretStats,
            ObservableCollection<IPvPBoostProvider> localBoostProviders,
            ObservableCollection<IPvPBoostProvider> globalFireRateBoostProviders);
    }
}

