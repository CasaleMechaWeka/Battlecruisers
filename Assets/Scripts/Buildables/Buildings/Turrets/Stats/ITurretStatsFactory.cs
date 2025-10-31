using BattleCruisers.Buildables.Boost;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public interface ITurretStatsFactory
    {
        ITurretStats CreateBoostedTurretStats(
            ITurretStats baseTurretStats,
            ObservableCollection<IBoostProvider> localBoostProviders,
            List<ObservableCollection<IBoostProvider>> globalFireRateBoostProviders);
    }
}
