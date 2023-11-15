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
    public class PvPBoostedTurretStats : PvPBoostedBasicTurretStats<IPvPTurretStats>, IPvPTurretStats
    {
        private readonly IPvPBoostable _accuracyBoostable;
        private readonly IPvPBoostableGroup _accuracyBoostableGroup;

        public float Accuracy => Mathf.Clamp01(_accuracyBoostable.BoostMultiplier * _baseStats.Accuracy);
        public float TurretRotateSpeedInDegrees => _baseStats.TurretRotateSpeedInDegrees;
        public bool IsInBurst => _baseStats.IsInBurst;
        public int BurstSize => _baseStats.BurstSize;

        public PvPBoostedTurretStats(
            IPvPTurretStats baseStats,
            IPvPBoostFactory boostFactory,
            ObservableCollection<IPvPBoostProvider> localBoostProviders,
            ObservableCollection<IPvPBoostProvider> globalFireRateBoostProviders,
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
