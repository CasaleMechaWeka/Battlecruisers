using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Buildings.Turrets.Stats.Boosted;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.Stats.Boosted
{
    public class BoostedBasicTurretStatsTests : BoostedTurretStatsTestsBase<IBasicTurretStats>
    {
        [Test]
        public void Constructor()
        {
            CreateTurretStats();

            _boostFactory.Received().CreateBoostable();
            _boostFactory.Received().CreateBoostableGroup();
            _boostableGroup.Received().AddBoostable(_boostable);
            _boostableGroup.Received().AddBoostProvidersList(_localBoostProviders);
            _boostableGroup.Received().AddBoostProvidersList(_globalBoostProviders.DefenseFireRateBoostProviders);
        }

        [Test]
        public void Forwarding_Properties()
        {
            IBasicTurretStats boostedStats = CreateTurretStats();

            float expectedFireRate = _boostable.BoostMultiplier * _baseStats.FireRatePerS;
            Assert.IsTrue(Mathf.Approximately(expectedFireRate, boostedStats.FireRatePerS));

            float expectedMeanFireRate = _boostable.BoostMultiplier * _baseStats.MeanFireRatePerS;
            Assert.IsTrue(Mathf.Approximately(expectedMeanFireRate, boostedStats.MeanFireRatePerS));

            float expectedDuration = _baseStats.DurationInS / _boostable.BoostMultiplier;
            Assert.IsTrue(Mathf.Approximately(expectedDuration, boostedStats.DurationInS));

            Assert.AreEqual(_baseStats.RangeInM, boostedStats.RangeInM);
            Assert.AreEqual(_baseStats.MinRangeInM, boostedStats.MinRangeInM);
            Assert.AreEqual(_baseStats.AttackCapabilities, boostedStats.AttackCapabilities);
        }

        [Test]
        public void Forwarding_Methods()
        {
            IBasicTurretStats boostedStats = CreateTurretStats();

            boostedStats.MoveToNextDuration();
            _baseStats.Received().MoveToNextDuration();
        }

        private IBasicTurretStats CreateTurretStats()
        {
            return new BoostedBasicTurretStats<IBasicTurretStats>(_baseStats, _boostFactory, _localBoostProviders, _globalBoostProviders.DefenseFireRateBoostProviders);
        }
    }
}
