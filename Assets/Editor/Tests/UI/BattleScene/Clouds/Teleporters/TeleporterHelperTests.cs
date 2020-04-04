using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.BattleScene.Clouds.Teleporters;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.BattleScene.Clouds.Teleporters
{
    public class TeleporterHelperTests
    {
        private ITeleporterHelper _helper;
        private ICloudNEW _onScreenCloud, _offScreenCloud;

        [SetUp]
        public void TestSetup()
        {
            _helper = new TeleporterHelper();

            _onScreenCloud = Substitute.For<ICloudNEW>();
            Vector2 onScreenCloudSize = new Vector2(4, 1);
            _onScreenCloud.Size.Returns(onScreenCloudSize);

            _offScreenCloud = Substitute.For<ICloudNEW>();
            Vector2 offScreenCloudSize = new Vector2(2, 1);
            _offScreenCloud.Size.Returns(offScreenCloudSize);
        }

        [Test]
        public void ShouldTeleportCloud_True()
        {
            Vector2 cloudPosition = new Vector2(TeleporterHelper.MAX_X_POSITION_VISIBLE_BY_USER + _offScreenCloud.Size.x / 2 + 1, 0);
            _offScreenCloud.Position.Returns(cloudPosition);
            Assert.IsTrue(_helper.ShouldTeleportCloud(_offScreenCloud));
        }

        [Test]
        public void ShouldTeleportCloud_False()
        {
            Vector2 cloudPosition = new Vector2(TeleporterHelper.MAX_X_POSITION_VISIBLE_BY_USER + _offScreenCloud.Size.x / 2, 0);
            _offScreenCloud.Position.Returns(cloudPosition);
            Assert.IsFalse(_helper.ShouldTeleportCloud(_offScreenCloud));
        }

        [Test]
        public void FindTeleportTargetPosition_WrongCloudOrder_Throws()
        {
            _offScreenCloud.Position.Returns(new Vector2(-2, 0));
            _onScreenCloud.Position.Returns(new Vector2(2, 0));

            Assert.Throws<UnityAsserts.AssertionException>(() => _helper.FindTeleportTargetPosition(_onScreenCloud, _offScreenCloud));
        }

        [Test]
        public void FindTeleportTargetPosition()
        {
            // Arrange
            Vector2 onScreenCloudPosition = new Vector2(0, 0);
            _onScreenCloud.Position.Returns(onScreenCloudPosition);

            Vector2 offScreenCloudPosition = new Vector2(10, 3);
            _offScreenCloud.Position.Returns(offScreenCloudPosition);

            float distanceBetweenCloudsInM = _offScreenCloud.Position.x - _onScreenCloud.Position.x;
            float distanceToTeleportInM = 2 * distanceBetweenCloudsInM;
            float targetPositionX = _offScreenCloud.Position.x - distanceToTeleportInM;

            Vector2 expectedTargetPosition = new Vector2(targetPositionX, _offScreenCloud.Position.y);

            // Act
            Vector2 actualTargetPosition = _helper.FindTeleportTargetPosition(_onScreenCloud, _offScreenCloud);

            // Assert
            Assert.AreEqual(expectedTargetPosition, actualTargetPosition);
        }
    }
}