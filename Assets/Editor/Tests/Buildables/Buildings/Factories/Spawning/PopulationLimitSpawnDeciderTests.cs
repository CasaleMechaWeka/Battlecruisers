using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Cruisers.Construction;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.Buildings.Factories.Spawning
{
    public class PopulationLimitSpawnDeciderTests
    {
        private IUnitSpawnDecider _unitSpawnDecider;
        private ICruiserUnitMonitor _unitMonitor;
        private int _populationLimit;

        [SetUp]
        public void TestSetup()
        {
            _unitMonitor = Substitute.For<ICruiserUnitMonitor>();
            _populationLimit = 3;

            _unitSpawnDecider = new PopulationLimitSpawnDecider(_unitMonitor, _populationLimit);
        }

        [Test]
        public void CanSpawnUnit_True()
        {
            _unitMonitor.AliveUnits.Count.Returns(_populationLimit - 1);
            Assert.IsTrue(_unitSpawnDecider.CanSpawnUnit(default));
        }

        [Test]
        public void CanSpawnUnit_False()
        {
            _unitMonitor.AliveUnits.Count.Returns(_populationLimit);
            Assert.IsFalse(_unitSpawnDecider.CanSpawnUnit(default));
        }
    }
}