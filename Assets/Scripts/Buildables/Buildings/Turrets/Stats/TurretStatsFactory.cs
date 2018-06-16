using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.Stats.Boosted;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class TurretStatsFactory : ITurretStatsFactory
    {
        private readonly IBoostFactory _boostFactory;
        private readonly IObservableCollection<IBoostProvider> _localBoostProviders;
        private readonly IGlobalBoostProviders _globalBoostProviders;

        public TurretStatsFactory(
            IBoostFactory boostFactory,
            IObservableCollection<IBoostProvider> localBoostProviders,
            IGlobalBoostProviders globalBoostProviders)
        {
            Helper.AssertIsNotNull(boostFactory, localBoostProviders, globalBoostProviders);

            _boostFactory = boostFactory;
            _localBoostProviders = localBoostProviders;
            _globalBoostProviders = globalBoostProviders;
        }

        public ITurretStats CreateBoostedTurretStats(ITurretStats baseTurretStats)
        {
            return new BoostedTurretStats(baseTurretStats, _boostFactory, _localBoostProviders, _globalBoostProviders);
        }
    }
}
