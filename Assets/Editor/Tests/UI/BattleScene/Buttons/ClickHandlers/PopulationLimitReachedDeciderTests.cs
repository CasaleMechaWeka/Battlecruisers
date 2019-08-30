using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.BattleScene.Buttons.ClickHandlers
{
    public class PopulationLimitReachedDeciderTests
    {
        private IPopulationLimitReachedDecider _decider;
        private IPopulationLimitMonitor _populationLimitMonitor;
        private IFactory _factory;
        private IUnit _unit;

        [SetUp]
        public void TestSetup()
        {
            _populationLimitMonitor = Substitute.For<IPopulationLimitMonitor>();
            _decider = new PopulationLimitReachedDecider(_populationLimitMonitor);

            _factory = Substitute.For<IFactory>();
            _unit = Substitute.For<IUnit>();

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void ShouldPlayPopulationLimitReachedWarning_NullParameter_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _decider.ShouldPlayPopulationLimitReachedWarning(null));
        }

        [Test]
        public void ShouldPlayPopulationLimitReachedWarning_PopulationLimitNotReached()
        {
            _populationLimitMonitor.IsPopulationLimitReached.Returns(false);
            Assert.IsFalse(_decider.ShouldPlayPopulationLimitReachedWarning(_factory));
        }

        [Test]
        public void ShouldPlayPopulationLimitReachedWarning_PopulationLimitReached_UnitUnderConstruction()
        {
            _populationLimitMonitor.IsPopulationLimitReached.Returns(true);
            _factory.UnitUnderConstruction.Returns(_unit);
            Assert.IsFalse(_decider.ShouldPlayPopulationLimitReachedWarning(_factory));
        }

        [Test]
        public void ShouldPlayPopulationLimitReachedWarning_PopulationLimitReached_NoUnitUnderConstruction_UnitPaused()
        {
            _populationLimitMonitor.IsPopulationLimitReached.Returns(true);
            _factory.UnitUnderConstruction.Returns((IUnit)null);
            _factory.IsUnitPaused.Value.Returns(true);
            Assert.IsFalse(_decider.ShouldPlayPopulationLimitReachedWarning(_factory));
        }

        [Test]
        public void ShouldPlayPopulationLimitReachedWarning_PopulationLimitReached_NoUnitUnderConstruction_UnitNotPaused()
        {
            _populationLimitMonitor.IsPopulationLimitReached.Returns(true);
            _factory.UnitUnderConstruction.Returns((IUnit)null);
            _factory.IsUnitPaused.Value.Returns(false);
            Assert.IsTrue(_decider.ShouldPlayPopulationLimitReachedWarning(_factory));
        }
    }
}