using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Buildings.Turrets.Stats.Boosted;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.Stats.Boosted
{
    public class BoostedBasicTurretStatsTests
    {
        private IBasicTurretStats _boostedStats;

        private IBasicTurretStats _baseStats;
        private IBoostFactory _boostFactory;
        private IGlobalBoostProviders _globalBoostProviders;
        private IObservableCollection<IBoostProvider> _localBoostProviders;
        private IBoostable _fireRateBoostable;
        private IBoostableGroup _fireRateBoostabelGroup;

        [SetUp]
        public void SetuUp()
        {
            _baseStats = Substitute.For<IBasicTurretStats>();
            ReadOnlyCollection<TargetType> attackCapabilities = new ReadOnlyCollection<TargetType>(new List<TargetType>());
            _baseStats.AttackCapabilities.Returns(attackCapabilities);
            _baseStats.DurationInS.Returns(0.2f);
            _baseStats.FireRatePerS.Returns(0.3f);
            _baseStats.MeanFireRatePerS.Returns(0.4f);
            _baseStats.MinRangeInM.Returns(0.5f);
            _baseStats.RangeInM.Returns(0.6f);

            _boostFactory = Substitute.For<IBoostFactory>();

            _fireRateBoostable = Substitute.For<IBoostable>();
            _fireRateBoostable.BoostMultiplier.Returns(3.3f);
            _boostFactory.CreateBoostable().Returns(_fireRateBoostable);

            _fireRateBoostabelGroup = Substitute.For<IBoostableGroup>();
            _boostFactory.CreateBoostableGroup().Returns(_fireRateBoostabelGroup);

            _globalBoostProviders = Helper.CreateGlobalBoostProviders();

            _localBoostProviders = new ObservableCollection<IBoostProvider>();
        }

        [Test]
        public void Constructor_NoLocalBoosts()
        {
            _boostedStats = new BoostedBasicTurretStats<IBasicTurretStats>(_baseStats, _boostFactory, _globalBoostProviders);

            _boostFactory.Received().CreateBoostable();
            _boostFactory.Received().CreateBoostableGroup();
            _fireRateBoostabelGroup.Received().AddBoostable(_fireRateBoostable);
            _fireRateBoostabelGroup.Received().AddBoostProvidersList(_globalBoostProviders.TurretFireRateBoostProviders);
        }

        [Test]
        public void Constructor_WithLocalBoosts()
        {
            _boostedStats = new BoostedBasicTurretStats<IBasicTurretStats>(_baseStats, _boostFactory, _globalBoostProviders, _localBoostProviders);

            _fireRateBoostabelGroup.Received().AddBoostProvidersList(_localBoostProviders);
        }

        [Test]
        public void Forwarding_Properties()
        {
            Constructor_NoLocalBoosts();

            float expectedFireRate = _fireRateBoostable.BoostMultiplier * _baseStats.FireRatePerS;
            Assert.IsTrue(Mathf.Approximately(expectedFireRate, _boostedStats.FireRatePerS));
            Assert.IsTrue(Mathf.Approximately(expectedFireRate, _boostedStats.MeanFireRatePerS));

            Assert.AreEqual(_baseStats.RangeInM, _boostedStats.RangeInM);
            Assert.AreEqual(_baseStats.MinRangeInM, _boostedStats.MinRangeInM);
            Assert.AreEqual(_baseStats.AttackCapabilities, _boostedStats.AttackCapabilities);

            float expectedDuration = 1 / expectedFireRate;
            Assert.IsTrue(Mathf.Approximately(expectedDuration, _boostedStats.DurationInS));
        }

        [Test]
        public void Forwarding_Methods()
        {
            Constructor_NoLocalBoosts();

            _boostedStats.MoveToNextDuration();
            _baseStats.Received().MoveToNextDuration();
        }
    }
}
