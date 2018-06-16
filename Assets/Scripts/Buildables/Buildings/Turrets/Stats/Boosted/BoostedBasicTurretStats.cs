using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats.Boosted
{
    // FELIX  Avoid duplicate code between Boosted classes :)

    // FELIX  Test

    /// <summary>
    /// Wraps IBasicTurretStats, adding boosters.
    /// </summary>
    public class BoostedBasicTurretStats<TStats> : IBasicTurretStats where TStats : IBasicTurretStats
    {
        protected readonly TStats _baseStats;
        private readonly IBoostable _fireRateBoostable;
        private readonly IBoostableGroup _fireRateBoostabelGroup;

        public float FireRatePerS { get { return _fireRateBoostable.BoostMultiplier * _baseStats.FireRatePerS; } }
        public float RangeInM { get { return _baseStats.RangeInM; } }
        public float MinRangeInM { get { return _baseStats.MinRangeInM; } }
        public float MeanFireRatePerS { get { return _baseStats.MeanFireRatePerS; } }
        public ReadOnlyCollection<TargetType> AttackCapabilities { get { return _baseStats.AttackCapabilities; } }
        public float DurationInS { get { return 1 / FireRatePerS; } }

        public BoostedBasicTurretStats(
            TStats baseStats,
            IBoostFactory boostFactory,
            IObservableCollection<IBoostProvider> localBoostProviders,
            IGlobalBoostProviders globalBoostProviders)
        {
            Helper.AssertIsNotNull(baseStats, boostFactory, localBoostProviders, globalBoostProviders);

            _baseStats = baseStats;

            _fireRateBoostable = boostFactory.CreateBoostable();
            _fireRateBoostabelGroup = boostFactory.CreateBoostableGroup();
            _fireRateBoostabelGroup.AddBoostable(_fireRateBoostable);
            _fireRateBoostabelGroup.AddBoostProvidersList(globalBoostProviders.TurretFireRateBoostProviders);

            // Assign local boost to fire rate.  Can easily be changed to boost
            // another statistic :)
            _fireRateBoostabelGroup.AddBoostProvidersList(localBoostProviders);
        }

        public void MoveToNextDuration()
        {
            throw new System.NotImplementedException();
        }
    }
}
