using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats.Boosted
{
    /// <summary>
    /// Wraps IBasicTurretStats, adding boosters.
    /// </summary>
    public class BoostedBasicTurretStats<TStats> : IBasicTurretStats where TStats : IBasicTurretStats
    {
        protected readonly TStats _baseStats;
        private readonly IBoostable _fireRateBoostable;
        private readonly IBoostableGroup _fireRateBoostabelGroup;

        public float FireRatePerS => _fireRateBoostable.BoostMultiplier * _baseStats.FireRatePerS;
        public float RangeInM => _baseStats.RangeInM;
        public float MinRangeInM => _baseStats.MinRangeInM;
        public float MeanFireRatePerS => _fireRateBoostable.BoostMultiplier * _baseStats.MeanFireRatePerS;
        public ReadOnlyCollection<TargetType> AttackCapabilities => _baseStats.AttackCapabilities;
        public float DurationInS => _baseStats.DurationInS / _fireRateBoostable.BoostMultiplier;

        public BoostedBasicTurretStats(
            TStats baseStats,
            IBoostFactory boostFactory,
            ObservableCollection<IBoostProvider> localBoostProviders,
            ObservableCollection<IBoostProvider> globalFireRateBoostProviders)
        {
            Helper.AssertIsNotNull(baseStats, boostFactory, localBoostProviders, globalFireRateBoostProviders);

            _baseStats = baseStats;

            _fireRateBoostable = boostFactory.CreateBoostable();
            _fireRateBoostabelGroup = boostFactory.CreateBoostableGroup();
            _fireRateBoostabelGroup.AddBoostable(_fireRateBoostable);
            _fireRateBoostabelGroup.AddBoostProvidersList(globalFireRateBoostProviders);
            _fireRateBoostabelGroup.AddBoostProvidersList(localBoostProviders);
        }

        public void MoveToNextDuration()
        {
            _baseStats.MoveToNextDuration();
        }
    }
}
