using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats.Boosted
{
    /// <summary>
    /// Wraps ITurretStats, adding boosters.
    /// </summary>
    public class PvPBoostedTurretStats : PvPBoostedBasicTurretStats<ITurretStats>, ITurretStats
    {
        private readonly IBoostable _accuracyBoostable;
        private readonly IBoostableGroup _accuracyBoostableGroup;

        public float Accuracy => Mathf.Clamp01(_accuracyBoostable.BoostMultiplier * _baseStats.Accuracy);
        public float TurretRotateSpeedInDegrees => _baseStats.TurretRotateSpeedInDegrees;
        public bool IsInBurst => _baseStats.IsInBurst;
        public int BurstSize => _baseStats.BurstSize;

        public PvPBoostedTurretStats(
            ITurretStats baseStats,
            ObservableCollection<IBoostProvider> localBoostProviders,
            List<ObservableCollection<IBoostProvider>> globalFireRateBoostProviders,
            GlobalBoostProviders globalBoostProviders)
            : base(baseStats, localBoostProviders, globalFireRateBoostProviders)
        {
            Assert.IsNotNull(globalBoostProviders);

            _accuracyBoostable = new Boostable(1);
            _accuracyBoostableGroup = new BoostableGroup();
            _accuracyBoostableGroup.AddBoostable(_accuracyBoostable);
            _accuracyBoostableGroup.AddBoostProvidersList(globalBoostProviders.TurretAccuracyBoostProviders);
        }
    }
}
