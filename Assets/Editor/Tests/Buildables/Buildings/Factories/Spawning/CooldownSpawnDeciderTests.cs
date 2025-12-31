using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.Buildings.Factories.Spawning
{
    public class CooldownSpawnDeciderTests
    {
        private IUnitSpawnDecider _unitSpawnDecider;
        private IUnitSpawnTimer _unitSpawnTimer;

        [SetUp]
        public void TestSetup()
        {
            _unitSpawnTimer = Substitute.For<IUnitSpawnTimer>();
            _unitSpawnDecider = new CooldownSpawnDecider(_unitSpawnTimer);
        }

        [Test]
        public void CanSpawnUnit_NoUnitRecentlyChosen_FactoryCooldownDone()
        {
            _unitSpawnTimer.TimeSinceUnitWasChosenInS.Returns(CooldownSpawnDecider.MIN_BUILD_BREAK_IN_S + 0.1f);
            _unitSpawnTimer.TimeSinceFactoryWasClearInS.Returns(CooldownSpawnDecider.MIN_BUILD_BREAK_IN_S + 0.1f);

            Assert.IsTrue(_unitSpawnDecider.CanSpawnUnit(default));
        }

        [Test]
        public void CanSpawnUnit_NoUnitRecentlyChosen_FactoryCooldownNotDone()
        {
            _unitSpawnTimer.TimeSinceUnitWasChosenInS.Returns(CooldownSpawnDecider.MIN_BUILD_BREAK_IN_S + 0.1f);
            _unitSpawnTimer.TimeSinceFactoryWasClearInS.Returns(CooldownSpawnDecider.MIN_BUILD_BREAK_IN_S - 0.1f);

            Assert.IsFalse(_unitSpawnDecider.CanSpawnUnit(default));
        }

        [Test]
        public void CanSpawnUnit_UnitRecentlyChosen_FactoryCooldownDone()
        {
            _unitSpawnTimer.TimeSinceUnitWasChosenInS.Returns(CooldownSpawnDecider.MIN_BUILD_BREAK_IN_S - 0.1f);
            _unitSpawnTimer.TimeSinceFactoryWasClearInS.Returns(CooldownSpawnDecider.MIN_BUILD_BREAK_IN_S + 0.1f);

            Assert.IsTrue(_unitSpawnDecider.CanSpawnUnit(default));
        }

        [Test]
        public void CanSpawnUnit_UnitRecentlyChosen_FactoryCooldownNotDone()
        {
            _unitSpawnTimer.TimeSinceUnitWasChosenInS.Returns(CooldownSpawnDecider.MIN_BUILD_BREAK_IN_S - 0.1f);
            _unitSpawnTimer.TimeSinceFactoryWasClearInS.Returns(CooldownSpawnDecider.MIN_BUILD_BREAK_IN_S - 0.1f);

            Assert.IsTrue(_unitSpawnDecider.CanSpawnUnit(default));
        }
    }
}