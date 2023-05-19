using System.Collections.ObjectModel;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats.Boosted
{
    /// <summary>
    /// Wraps IBasicTurretStats, adding boosters.
    /// </summary>
    public class PvPBoostedBasicTurretStats<TStats> : IPvPBasicTurretStats where TStats : IPvPBasicTurretStats
    {
        protected readonly TStats _baseStats;
        private readonly IPvPBoostable _fireRateBoostable;
        private readonly IPvPBoostableGroup _fireRateBoostabelGroup;

        public float FireRatePerS => _fireRateBoostable.BoostMultiplier * _baseStats.FireRatePerS;
        public float RangeInM => _baseStats.RangeInM;
        public float MinRangeInM => _baseStats.MinRangeInM;
        public float MeanFireRatePerS => _fireRateBoostable.BoostMultiplier * _baseStats.MeanFireRatePerS;
        public ReadOnlyCollection<PvPTargetType> AttackCapabilities => _baseStats.AttackCapabilities;
        public float DurationInS => _baseStats.DurationInS / _fireRateBoostable.BoostMultiplier;

        public PvPBoostedBasicTurretStats(
            TStats baseStats,
            IPvPBoostFactory boostFactory,
            ObservableCollection<IPvPBoostProvider> localBoostProviders,
            ObservableCollection<IPvPBoostProvider> globalFireRateBoostProviders)
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
