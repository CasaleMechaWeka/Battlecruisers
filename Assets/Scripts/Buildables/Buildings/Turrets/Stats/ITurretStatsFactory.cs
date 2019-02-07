using BattleCruisers.Buildables.Boost;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public interface ITurretStatsFactory
    {
        ITurretStats CreateBoostedTurretStats(
            ITurretStats baseTurretStats, 
            IObservableCollection<IBoostProvider> localBoostProviders,
            IObservableCollection<IBoostProvider> globalFireRateBoostProviders);
    }
}
