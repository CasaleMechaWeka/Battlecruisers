using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Buildings.Turrets.Stats.Boosted;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.Stats.Boosted
{
    public class BoostedTurretStatsTests : BoostedTurretStatsTestsBase<ITurretStats>
    {
        private ITurretStats _boostedStats;
        private IObservableCollection<IBoostProvider> _localBoostProviders, _fireRateGlobalBoostProviders;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _baseStats.TurretRotateSpeedInDegrees.Returns(1.2f);
            _baseStats.IsInBurst.Returns(true);
            _baseStats.BurstSize.Returns(7);

            _localBoostProviders = new ObservableCollection<IBoostProvider>();
            _fireRateGlobalBoostProviders = new ObservableCollection<IBoostProvider>();

            _boostedStats = new BoostedTurretStats(_baseStats, _boostFactory, _localBoostProviders, _fireRateGlobalBoostProviders, _globalBoostProviders);
        }

        [Test]
        public void Constructor()
        {
            // Accuracy
            _boostableGroup.Received().AddBoostable(_boostable);
            _boostableGroup.Received().AddBoostProvidersList(_globalBoostProviders.TurretAccuracyBoostProviders);
        }

        [Test]
        public void Forwarding_SimpleProperites()
        {
            Assert.AreEqual(_baseStats.TurretRotateSpeedInDegrees, _boostedStats.TurretRotateSpeedInDegrees);
            Assert.AreEqual(_baseStats.IsInBurst, _boostedStats.IsInBurst);
            Assert.AreEqual(_baseStats.BurstSize, _boostedStats.BurstSize);
        }

        #region Accuracy
        [Test]
        public void Forwarding_Accuracy_Unclamped()
        {
            _boostable.BoostMultiplier.Returns(2);
            _baseStats.Accuracy.Returns(0.25f);

            Assert.IsTrue(Mathf.Approximately(0.5f, _boostedStats.Accuracy));
        }

        [Test]
        public void Forwarding_Accuracy_ClampedToMin()
        {
            _boostable.BoostMultiplier.Returns(-1);
            _baseStats.Accuracy.Returns(0.5f);

            // -1 * 0.5 = -0.5
            Assert.AreEqual(0, _boostedStats.Accuracy);
        }

        [Test]
        public void Forwarding_Accuracy_ClampedToMax()
        {
            _boostable.BoostMultiplier.Returns(4);
            _baseStats.Accuracy.Returns(0.5f);

            // 4 * 0.5 = 2
            Assert.AreEqual(1, _boostedStats.Accuracy);
        }
        #endregion Accuracy
    }
}
