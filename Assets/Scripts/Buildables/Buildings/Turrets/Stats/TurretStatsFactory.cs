using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats.Boosted;
using BattleCruisers.Utils;
using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class TurretStatsFactory : ITurretStatsFactory
    {
        private readonly GlobalBoostProviders _globalBoostProviders;

        public TurretStatsFactory(GlobalBoostProviders globalBoostProviders)
        {
            Helper.AssertIsNotNull(globalBoostProviders);

            _globalBoostProviders = globalBoostProviders;
        }

        public ITurretStats CreateBoostedTurretStats(
            ITurretStats baseTurretStats,
            ObservableCollection<IBoostProvider> localBoostProviders,
            ObservableCollection<IBoostProvider> globalFireRateBoostProviders)
        {
            return new BoostedTurretStats(baseTurretStats, localBoostProviders, globalFireRateBoostProviders, _globalBoostProviders);
        }
    }
}
