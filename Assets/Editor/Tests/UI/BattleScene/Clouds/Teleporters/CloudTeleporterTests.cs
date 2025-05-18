using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.Utils.BattleScene.Update;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.BattleScene.Clouds.Teleporters
{
    public class CloudTeleporterTests
    {
        private CloudTeleporter _teleporter;
        private IUpdater _updater;
        private CloudController _leftCloud, _rightCloud;

        [SetUp]
        public void TestSetup()
        {
            _updater = Substitute.For<IUpdater>();
            _leftCloud = Substitute.For<CloudController>();
            _leftCloud.Position.Returns(new Vector3(-1, 0, 0));
            _rightCloud = Substitute.For<CloudController>();
            _rightCloud.Position.Returns(new Vector3(1, 0, 0));

            _teleporter = new CloudTeleporter(_updater, _leftCloud, _rightCloud);
        }

        [Test]
        public void _updater_Updated_ShouldNotTeleport()
        {
            _teleporter.ShouldTeleportCloud(_rightCloud).Returns(false);
            _updater.Updated += Raise.Event();
            _teleporter.DidNotReceiveWithAnyArgs().FindTeleportTargetPosition(default, default);
        }

        [Test]
        public void _updater_Updated_ShouldTeleport()
        {
            // First teleport
            _teleporter.ShouldTeleportCloud(_rightCloud).Returns(true);
            Vector3 teleportTargetPosition = new Vector3(17, 34, -7);
            _teleporter.FindTeleportTargetPosition(_leftCloud, _rightCloud).Returns(teleportTargetPosition);

            _updater.Updated += Raise.Event();

            _rightCloud.Received().Position = teleportTargetPosition;

            // Second teleport, to assert that clouds were switched
            _teleporter.ShouldTeleportCloud(_leftCloud).Returns(true);
            Vector3 teleportTargetPosition2 = new Vector3(1, 3, -5);
            _teleporter.FindTeleportTargetPosition(_rightCloud, _leftCloud).Returns(teleportTargetPosition2);

            _updater.Updated += Raise.Event();

            _leftCloud.Received().Position = teleportTargetPosition2;
        }
    }
}