using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public interface IPvPTurretStatsFactory
    {
        ITurretStats CreateBoostedTurretStats(
            ITurretStats baseTurretStats,
            ObservableCollection<IBoostProvider> localBoostProviders,
            ObservableCollection<IBoostProvider> globalFireRateBoostProviders);
    }
}

