using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.BattleScene.Clouds.Teleporters;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.BattleScene.Clouds.Teleporters
{
    public class PositiveVelocityTeleporterTests
    {
        private ICloudTeleporter _teleporter;

        private ICloud _cloud;
        private ICloudStats _cloudStats;
        private float _adjustedDisappearLine, _adjustedReappearLine;

        [SetUp]
        public void SetuUp()
        {
            _cloud = Substitute.For<ICloud>();
            _cloudStats = Substitute.For<ICloudStats>();

            Vector2 cloudSize = new Vector2(4, 2);
            _cloud.Size.Returns(cloudSize);

            _cloudStats.DisappearLineInM.Returns(10);
            _cloudStats.ReappaerLineInM.Returns(-10);

            _teleporter = new PositiveVelocityTeleporter(_cloud, _cloudStats);

            _adjustedDisappearLine = 14;
            _adjustedReappearLine = -14;
        }

        [Test]
        public void ShouldTeleportCloud_False()
        {
            _cloud.Position = new Vector2(_adjustedDisappearLine, 0);
            Assert.IsFalse(_teleporter.ShouldTeleportCloud());
        }

        [Test]
        public void ShouldTeleportCloud_True()
        {
            _cloud.Position = new Vector2(_adjustedDisappearLine + 0.01f, 0);
            Assert.IsTrue(_teleporter.ShouldTeleportCloud());
        }

        [Test]
        public void TeleportCloud()
        {
            _cloud.Position = new Vector2(_adjustedDisappearLine + 0.01f, 7);

            _teleporter.TeleportCloud();

            Assert.AreEqual(new Vector2(_adjustedReappearLine, 7), _cloud.Position);
        }
    }
}
