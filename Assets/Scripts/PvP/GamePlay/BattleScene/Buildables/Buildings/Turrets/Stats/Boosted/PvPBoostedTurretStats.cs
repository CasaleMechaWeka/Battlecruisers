using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
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
        private readonly IPvPBoostableGroup _accuracyBoostableGroup;

        public float Accuracy => Mathf.Clamp01(_accuracyBoostable.BoostMultiplier * _baseStats.Accuracy);
        public float TurretRotateSpeedInDegrees => _baseStats.TurretRotateSpeedInDegrees;
        public bool IsInBurst => _baseStats.IsInBurst;
        public int BurstSize => _baseStats.BurstSize;

        public PvPBoostedTurretStats(
            ITurretStats baseStats,
            IPvPBoostFactory boostFactory,
            ObservableCollection<IBoostProvider> localBoostProviders,
            ObservableCollection<IBoostProvider> globalFireRateBoostProviders,
            IPvPGlobalBoostProviders globalBoostProviders)
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
