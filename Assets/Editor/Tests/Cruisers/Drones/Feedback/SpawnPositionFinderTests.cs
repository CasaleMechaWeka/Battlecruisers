using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
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
            _positionFinder = new SpawnPositionFinder(_random, Constants.WATER_LINE);

            _droneConsumerInfo = Substitute.For<IDroneConsumerInfo>();
        }

        [Test]
        public void FindSpawnPosition_NotCapped()
        {
            _droneConsumerInfo.Position.Returns(new Vector2(12, 0));
            _droneConsumerInfo.Size.Returns(new Vector2(2, 1));

            float xDeltaInM = _droneConsumerInfo.Size.x / 2;
            IRange<float> xPositionRange
                = new Range<float>(
                    _droneConsumerInfo.Position.x - xDeltaInM,
                    _droneConsumerInfo.Position.x + xDeltaInM);

            float yDeltaInM = _droneConsumerInfo.Size.y / 2;
            IRange<float> yPositionRange
                = new Range<float>(
                    _droneConsumerInfo.Position.y - yDeltaInM,
                    _droneConsumerInfo.Position.y + yDeltaInM);

            Vector2 expectedPosition = new Vector2(17, 71);
            _random.Range(xPositionRange).Returns(expectedPosition.x);
            _random.Range(yPositionRange).Returns(expectedPosition.y);

            Assert.AreEqual(expectedPosition, _positionFinder.FindSpawnPosition(_droneConsumerInfo));
        }


        [Test]
        public void FindSpawnPosition_Capped()
        {
            _droneConsumerInfo.Position.Returns(new Vector2(12, -4));
            _droneConsumerInfo.Size.Returns(new Vector2(2, 1));
            // -4 - 1 < WATER_LINE (-1.4)!
            // -4 + 1 < WATER_LINE (-1.4)!

            float xDeltaInM = _droneConsumerInfo.Size.x / 2;
            IRange<float> xPositionRange
                = new Range<float>(
                    _droneConsumerInfo.Position.x - xDeltaInM,
                    _droneConsumerInfo.Position.x + xDeltaInM);

            float yDeltaInM = _droneConsumerInfo.Size.y / 2;
            IRange<float> yPositionRange
                = new Range<float>(
                    Constants.WATER_LINE,
                    Constants.WATER_LINE);

            Vector2 expectedPosition = new Vector2(17, 71);
            _random.Range(xPositionRange).Returns(expectedPosition.x);
            _random.Range(yPositionRange).Returns(expectedPosition.y);

            Assert.AreEqual(expectedPosition, _positionFinder.FindSpawnPosition(_droneConsumerInfo));
        }
    }
}