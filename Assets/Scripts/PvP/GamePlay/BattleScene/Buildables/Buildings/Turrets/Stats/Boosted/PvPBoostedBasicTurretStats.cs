using System.Collections.ObjectModel;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats.Boosted
{
    /// <summary>
    /// Wraps IBasicTurretStats, adding boosters.
    /// </summary>
    public class PvPBoostedBasicTurretStats<TStats> : IBasicTurretStats where TStats : IBasicTurretStats
    {
        protected readonly TStats _baseStats;
        private readonly IBoostable _fireRateBoostable;
        private readonly IPvPBoostableGroup _fireRateBoostabelGroup;

        public float FireRatePerS => _fireRateBoostable.BoostMultiplier * _baseStats.FireRatePerS;
        public float RangeInM => _baseStats.RangeInM;
        public float MinRangeInM => _baseStats.MinRangeInM;
        public float MeanFireRatePerS => _fireRateBoostable.BoostMultiplier * _baseStats.MeanFireRatePerS;
        public ReadOnlyCollection<TargetType> AttackCapabilities => _baseStats.AttackCapabilities;
        public float DurationInS => _baseStats.DurationInS / _fireRateBoostable.BoostMultiplier;

        public PvPBoostedBasicTurretStats(
            TStats baseStats,
            IPvPBoostFactory boostFactory,
            ObservableCollection<IBoostProvider> localBoostProviders,
            ObservableCollection<IBoostProvider> globalFireRateBoostProviders)
        {
            PvPHelper.AssertIsNotNull(baseStats, boostFactory, localBoostProviders, globalFireRateBoostProviders);

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
