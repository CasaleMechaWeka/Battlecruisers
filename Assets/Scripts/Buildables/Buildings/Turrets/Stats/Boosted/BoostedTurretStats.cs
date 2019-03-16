using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats.Boosted
{
    /// <summary>
    /// Wraps ITurretStats, adding boosters.
    /// </summary>
    public class BoostedTurretStats : BoostedBasicTurretStats<ITurretStats>, ITurretStats
    {
        private readonly IBoostable _accuracyBoostable;
        private readonly IBoostableGroup _accuracyBoostableGroup;

        public float Accuracy => Mathf.Clamp01(_accuracyBoostable.BoostMultiplier * _baseStats.Accuracy);
        public float TurretRotateSpeedInDegrees => _baseStats.TurretRotateSpeedInDegrees;
        public bool IsInBurst => _baseStats.IsInBurst;
        public int BurstSize => _baseStats.BurstSize;

        public BoostedTurretStats(
            ITurretStats baseStats,
            IBoostFactory boostFactory,
            ObservableCollection<IBoostProvider> localBoostProviders,
            ObservableCollection<IBoostProvider> globalFireRateBoostProviders,
            IGlobalBoostProviders globalBoostProviders)
            : base(baseStats, boostFactory, localBoostProviders, globalFireRateBoostProviders)
        {
            Assert.IsNotNull(globalBoostProviders);

            _accuracyBoostable = boostFactory.CreateBoostable();
            _accuracyBoostableGroup = boostFactory.CreateBoostableGroup();
            _accuracyBoostableGroup.AddBoostable(_accuracyBoostable);
            _accuracyBoostableGroup.AddBoostProvidersList(globalBoostProviders.TurretAccuracyBoostProviders);
        }
    }
}
