using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Utils;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Cruisers.Drones.Feedback
{
    public class SpawnPositionFinderTests
    {
        private ISpawnPositionFinder _positionFinder;
        private IRandomGenerator _random;
        private IDroneConsumerInfo _droneConsumerInfo;

        [SetUp]
        public void TestSetup()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _random = Substitute.For<IRandomGenerator>();
            _positionFinder = new SpawnPositionFinder(_random);

            _droneConsumerInfo = Substitute.For<IDroneConsumerInfo>();
            _droneConsumerInfo.Position.Returns(new Vector2(33, 22));
            _droneConsumerInfo.Size.Returns(new Vector2(17, -17));
        }

        [Test]
        public void FindSpawnPosition()
        {
            Vector2 randomElement = new Vector2(2, 4);
            _random.Range(-_droneConsumerInfo.Size.x / 2, _droneConsumerInfo.Size.x / 2).Returns(randomElement.x);
            _random.Range(-_droneConsumerInfo.Size.y / 2, _droneConsumerInfo.Size.y / 2).Returns(randomElement.y);
            Vector2 expectedPosition = _droneConsumerInfo.Position + randomElement;

            Assert.AreEqual(expectedPosition, _positionFinder.FindSpawnPosition(_droneConsumerInfo));
        }
    }
}