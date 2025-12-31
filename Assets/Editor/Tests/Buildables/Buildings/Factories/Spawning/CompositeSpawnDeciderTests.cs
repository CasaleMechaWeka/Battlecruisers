using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.Buildings.Factories.Spawning
{
    public class CompositeSpawnDeciderTests
    {
        private IUnitSpawnDecider _compositeDecider, _decider1, _decider2;
        private IUnit _unitToSpawn;

        [SetUp]
        public void TestSetup()
        {
            _decider1 = Substitute.For<IUnitSpawnDecider>();
            _decider2 = Substitute.For<IUnitSpawnDecider>();

            _compositeDecider = new CompositeSpawnDecider(_decider1, _decider2);

            _unitToSpawn = Substitute.For<IUnit>();
        }

        [Test]
        public void CanSpawnUnit_AllDecidersSayYes()
        {
            _decider1.CanSpawnUnit(_unitToSpawn).Returns(true);
            _decider2.CanSpawnUnit(_unitToSpawn).Returns(true);

            Assert.IsTrue(_compositeDecider.CanSpawnUnit(_unitToSpawn));
        }

        [Test]
        public void CanSpawnUnit_OneDeciderSaysNo()
        {
            _decider1.CanSpawnUnit(_unitToSpawn).Returns(true);
            _decider2.CanSpawnUnit(_unitToSpawn).Returns(false);

            Assert.IsFalse(_compositeDecider.CanSpawnUnit(_unitToSpawn));
        }
    }
}