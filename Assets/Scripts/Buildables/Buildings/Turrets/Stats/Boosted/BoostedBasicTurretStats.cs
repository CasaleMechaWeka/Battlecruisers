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

        public float FireRatePerS { get { return _fireRateBoostable.BoostMultiplier * _baseStats.FireRatePerS; } }
        public float RangeInM { get { return _baseStats.RangeInM; } }
        public float MinRangeInM { get { return _baseStats.MinRangeInM; } }
        public float MeanFireRatePerS { get { return _fireRateBoostable.BoostMultiplier * _baseStats.MeanFireRatePerS; } }
        public ReadOnlyCollection<TargetType> AttackCapabilities { get { return _baseStats.AttackCapabilities; } }
        public float DurationInS { get { return _baseStats.DurationInS / _fireRateBoostable.BoostMultiplier; } }

        public BoostedBasicTurretStats(
            TStats baseStats,
            IBoostFactory boostFactory,
            // FELIX  Remove :)  Pass globalAccuracyFireRAteBoostProvider instead :)
            IGlobalBoostProviders globalBoostProviders,
            IObservableCollection<IBoostProvider> localBoostProviders,
            IObservableCollection<IBoostProvider> globalFireRateBoostProviders)
        {
            Helper.AssertIsNotNull(baseStats, boostFactory, globalBoostProviders, globalFireRateBoostProviders);

            _baseStats = baseStats;

            _fireRateBoostable = boostFactory.CreateBoostable();
            _fireRateBoostabelGroup = boostFactory.CreateBoostableGroup();
            _fireRateBoostabelGroup.AddBoostable(_fireRateBoostable);
            _fireRateBoostabelGroup.AddBoostProvidersList(globalBoostProviders.TurretFireRateBoostProviders);

            // Only building turret stats will potentially have local boosters
            // from their slots.  Turret stats for barrels on units will not have
            // any local boosters.
            // FELIX  Pass DummyObservableCollection to avoid null check :)
            if (localBoostProviders != null)
            {
                // Assign local boost to fire rate.  Can easily be changed to boost
                // another statistic :)
                _fireRateBoostabelGroup.AddBoostProvidersList(localBoostProviders);
            }
        }

        public void MoveToNextDuration()
        {
            _baseStats.MoveToNextDuration();
        }
    }
}
