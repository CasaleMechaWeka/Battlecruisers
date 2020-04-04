using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.BattleScene.Clouds
{
    public class CloudRandomiserTests
    {
        private ICloudRandomiser _cloudRandomiser;
        private IRandomGenerator _random;
        private IRange<float> _rightCloudValidXPositions;
        private ICloud _leftCloud, _rightCloud;

        [SetUp]
        public void TestSetup()
        {
            _random = Substitute.For<IRandomGenerator>();
            _rightCloudValidXPositions = new Range<float>(0, 4);

            _cloudRandomiser = new CloudRandomiser(_random, _rightCloudValidXPositions);

            _leftCloud = Substitute.For<ICloud>();
            _leftCloud.Position.Returns(new Vector2(-2, 1));

            _rightCloud = Substitute.For<ICloud>();
            _rightCloud.Position.Returns(new Vector2(2, 2));
        }

        [Test]
        public void RandomiseStartingPosition_WrongCloudOrder_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _cloudRandomiser.RandomiseStartingPosition(_rightCloud, _leftCloud));
        }

        [Test]
        public void RandomiseStartingPosition()
        {
            // Arrange
            float distanceBetweenCloudsInM = _rightCloud.Position.x - _leftCloud.Position.x;
            _random.Range(_rightCloudValidXPositions).Returns(-17);

            float rightCloudXPosition = -17;
            Vector2 expectedRightCloudPosition = new Vector2(rightCloudXPosition, _rightCloud.Position.y);

            float leftCloudXPosition = rightCloudXPosition - distanceBetweenCloudsInM;
            Vector2 expectedLeftCloudPosition = new Vector2(leftCloudXPosition, _leftCloud.Position.y);

            // Act
            _cloudRandomiser.RandomiseStartingPosition(_leftCloud, _rightCloud);

            // Assert
            _rightCloud.Received().Position = expectedRightCloudPosition;
            _leftCloud.Received().Position = expectedLeftCloudPosition;
        }
    }
}