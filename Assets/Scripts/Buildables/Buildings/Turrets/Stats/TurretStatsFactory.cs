using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats.Boosted;
using BattleCruisers.Utils;
using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class TurretStatsFactory : ITurretStatsFactory
    {
        private readonly IBoostFactory _boostFactory;
        private readonly IGlobalBoostProviders _globalBoostProviders;

        public TurretStatsFactory(IBoostFactory boostFactory, IGlobalBoostProviders globalBoostProviders)
        {
            Helper.AssertIsNotNull(boostFactory, globalBoostProviders);

            _boostFactory = boostFactory;
            _globalBoostProviders = globalBoostProviders;
        }

        public ITurretStats CreateBoostedTurretStats(
            ITurretStats baseTurretStats, 
            ObservableCollection<IBoostProvider> localBoostProviders,
            ObservableCollection<IBoostProvider> globalFireRateBoostProviders)
        {
            return new BoostedTurretStats(baseTurretStats, _boostFactory, localBoostProviders, globalFireRateBoostProviders, _globalBoostProviders);
        }
    }
}
