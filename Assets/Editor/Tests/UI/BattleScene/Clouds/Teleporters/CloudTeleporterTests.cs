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
        private CloudTeleporter _teleporter;
        private IUpdater _updater;
        private ITeleporterHelper _teleporterHelper;
        private ICloud _leftCloud, _rightCloud;

        [SetUp]
        public void TestSetup()
        {
            _updater = Substitute.For<IUpdater>();
            _teleporterHelper = Substitute.For<ITeleporterHelper>();
            _leftCloud = Substitute.For<ICloud>();
            _leftCloud.Position.Returns(new Vector3(-1, 0, 0));
            _rightCloud = Substitute.For<ICloud>();
            _rightCloud.Position.Returns(new Vector3(1, 0, 0));

            _teleporter = new CloudTeleporter(_updater, _teleporterHelper, _leftCloud, _rightCloud);
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
            Vector3 teleportTargetPosition = new Vector3(17, 34, -7);
            _teleporterHelper.FindTeleportTargetPosition(_leftCloud, _rightCloud).Returns(teleportTargetPosition);

            _updater.Updated += Raise.Event();

            _rightCloud.Received().Position = teleportTargetPosition;

            // Second teleport, to assert that clouds were switched
            _teleporterHelper.ShouldTeleportCloud(_leftCloud).Returns(true);
            Vector3 teleportTargetPosition2 = new Vector3(1, 3, -5);
            _teleporterHelper.FindTeleportTargetPosition(_rightCloud, _leftCloud).Returns(teleportTargetPosition2);

            _updater.Updated += Raise.Event();

            _leftCloud.Received().Position = teleportTargetPosition2;
        }
    }
}