using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.BattleScene.Clouds.Teleporters;
using BattleCruisers.Utils.BattleScene.Update;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.BattleScene.Clouds.Teleporters
{
    public class CloudTeleporterTests
    {
        private CloudTeleporterNEW _teleporter;
        private IUpdater _updater;
        private ITeleporterHelper _teleporterHelper;
        private ICloudNEW _leftCloud, _rightCloud;

        [SetUp]
        public void TestSetup()
        {
            _updater = Substitute.For<IUpdater>();
            _teleporterHelper = Substitute.For<ITeleporterHelper>();
            _leftCloud = Substitute.For<ICloudNEW>();
            _leftCloud.Position.Returns(new Vector2(-1, 0));
            _rightCloud = Substitute.For<ICloudNEW>();
            _rightCloud.Position.Returns(new Vector2(1, 0));

            _teleporter = new CloudTeleporterNEW(_updater, _teleporterHelper, _leftCloud, _rightCloud);
        }

        [Test]
        public void _updater_Updated_ShouldNotTeleport()
        {
            _teleporterHelper.ShouldTeleportCloud(_rightCloud).Returns(false);
            _updater.Updated += Raise.Event();
            _teleporterHelper.DidNotReceiveWithAnyArgs().FindTeleportTargetPosition(default, default);
        }

        [Test]
        public void _updater_Updated_ShouldTeleport()
        {
            // First teleport
            _teleporterHelper.ShouldTeleportCloud(_rightCloud).Returns(true);
            Vector2 teleportTargetPosition = new Vector2(17, 34);
            _teleporterHelper.FindTeleportTargetPosition(_leftCloud, _rightCloud).Returns(teleportTargetPosition);

            _updater.Updated += Raise.Event();

            _rightCloud.Received().Position = teleportTargetPosition;

            // Second teleport, to assert that clouds were switched
            _teleporterHelper.ShouldTeleportCloud(_leftCloud).Returns(true);
            Vector2 teleportTargetPosition2 = new Vector2(1, 3);
            _teleporterHelper.FindTeleportTargetPosition(_rightCloud, _leftCloud).Returns(teleportTargetPosition2);

            _updater.Updated += Raise.Event();

            _leftCloud.Received().Position = teleportTargetPosition2;
        }
    }
}