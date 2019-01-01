using BattleCruisers.Cruisers.Fog;
using NUnit.Framework;

namespace BattleCruisers.Tests.Cruisers.Fog
{
    public class FogVisibilityDeciderTests
    {
        private IFogVisibilityDecider _decider;

        [SetUp]
        public void TestSetup()
        {
            _decider = new FogVisibilityDecider();
        }

        [Test]
        public void ShouldFogBeVisible_0StealthGenerators_0SpySatellites_ReturnsFalse()
        {
            Assert.IsFalse(_decider.ShouldFogBeVisible(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 0));
        }

        [Test]
        public void ShouldFogBeVisible_1StealthGenerators_0SpySatellites_ReturnsTrue()
        {
            Assert.IsTrue(_decider.ShouldFogBeVisible(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 0));
        }

        [Test]
        public void ShouldFogBeVisible_0StealthGenerators_1SpySatellites_ReturnsFalse()
        {
            Assert.IsFalse(_decider.ShouldFogBeVisible(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 1));
        }

        [Test]
        public void ShouldFogBeVisible_1StealthGenerators_1SpySatellites_ReturnsFalse()
        {
            Assert.IsFalse(_decider.ShouldFogBeVisible(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 1));
        }
    }
}