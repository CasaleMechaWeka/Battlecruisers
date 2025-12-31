using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Cruisers.Drones.Feedback
{
    public class SpawnPositionFinderTests
    {
        private SpawnPositionFinder _positionFinder;
        private DroneConsumerInfo _droneConsumerInfo;

        [SetUp]
        public void TestSetup()
        {
            _positionFinder = new SpawnPositionFinder(Constants.WATER_LINE);

            _droneConsumerInfo = Substitute.For<DroneConsumerInfo>();
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
            RandomGenerator.Range(xPositionRange).Returns(expectedPosition.x);
            RandomGenerator.Range(yPositionRange).Returns(expectedPosition.y);

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
            RandomGenerator.Range(xPositionRange).Returns(expectedPosition.x);
            RandomGenerator.Range(yPositionRange).Returns(expectedPosition.y);

            Assert.AreEqual(expectedPosition, _positionFinder.FindSpawnPosition(_droneConsumerInfo));
        }
    }
}