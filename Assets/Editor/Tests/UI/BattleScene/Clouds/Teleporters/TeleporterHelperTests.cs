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
        private ICloud _onScreenCloud, _offScreenCloud;

        [SetUp]
        public void TestSetup()
        {
            _helper = new TeleporterHelper();

            _onScreenCloud = Substitute.For<ICloud>();
            Vector2 onScreenCloudSize = new Vector2(4, 1);
            _onScreenCloud.Size.Returns(onScreenCloudSize);

            _offScreenCloud = Substitute.For<ICloud>();
            Vector2 offScreenCloudSize = new Vector2(2, 1);
            _offScreenCloud.Size.Returns(offScreenCloudSize);
        }

        [Test]
        public void ShouldTeleportCloud_True()
        {
            float xPosition = TeleporterHelper.MAX_X_POSITION_VISIBLE_BY_USER + _offScreenCloud.Size.x / 2 + 1;
            Vector3 cloudPosition = new Vector3(xPosition, 0, 10);
            _offScreenCloud.Position.Returns(cloudPosition);
            Assert.IsTrue(_helper.ShouldTeleportCloud(_offScreenCloud));
        }

        [Test]
        public void ShouldTeleportCloud_False()
        {
            float xPosition = TeleporterHelper.MAX_X_POSITION_VISIBLE_BY_USER + _offScreenCloud.Size.x / 2;
            Vector3 cloudPosition = new Vector3(xPosition, 0, 0);
            _offScreenCloud.Position.Returns(cloudPosition);
            Assert.IsFalse(_helper.ShouldTeleportCloud(_offScreenCloud));
        }

        [Test]
        public void FindTeleportTargetPosition_WrongCloudOrder_Throws()
        {
            _offScreenCloud.Position.Returns(new Vector3(-2, 0, 0));
            _onScreenCloud.Position.Returns(new Vector3(2, 0, 0));

            Assert.Throws<UnityAsserts.AssertionException>(() => _helper.FindTeleportTargetPosition(_onScreenCloud, _offScreenCloud));
        }

        [Test]
        public void FindTeleportTargetPosition()
        {
            // Arrange
            Vector3 onScreenCloudPosition = new Vector3(0, 0, -10);
            _onScreenCloud.Position.Returns(onScreenCloudPosition);

            Vector3 offScreenCloudPosition = new Vector3(10, 3, -10);
            _offScreenCloud.Position.Returns(offScreenCloudPosition);

            float distanceBetweenCloudsInM = _offScreenCloud.Position.x - _onScreenCloud.Position.x;
            float distanceToTeleportInM = 2 * distanceBetweenCloudsInM;
            float targetPositionX = _offScreenCloud.Position.x - distanceToTeleportInM;

            Vector3 expectedTargetPosition = new Vector3(targetPositionX, _offScreenCloud.Position.y, _offScreenCloud.Position.z);

            // Act
            Vector3 actualTargetPosition = _helper.FindTeleportTargetPosition(_onScreenCloud, _offScreenCloud);

            // Assert
            Assert.AreEqual(expectedTargetPosition, actualTargetPosition);
        }
    }
}