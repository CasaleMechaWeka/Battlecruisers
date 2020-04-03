using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.BattleScene.Clouds.Teleporters;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.BattleScene.Clouds.Teleporters
{
    public class NegativeVelocityTelporterTests
    {
        private ICloudTeleporter _teleporter;

        private ICloud _cloud;
        private ICloudStatsExtended _cloudStats;

        [SetUp]
        public void SetuUp()
        {
            _cloud = Substitute.For<ICloud>();
            _cloudStats = Substitute.For<ICloudStatsExtended>();

            Vector2 cloudSize = new Vector2(4, 2);
            _cloud.Size.Returns(cloudSize);

            _cloudStats.DisappearLineInM.Returns(-10);
            _cloudStats.ReappaerLineInM.Returns(10);

            _teleporter = new NegativeVelocityTeleporter(_cloud, _cloudStats);
        }

        [Test]
        public void ShouldTeleportCloud_False()
        {
            _cloud.Position = new Vector2(_cloudStats.DisappearLineInM, 0);
            Assert.IsFalse(_teleporter.ShouldTeleportCloud());
        }

        [Test]
        public void ShouldTeleportCloud_True()
        {
            _cloud.Position = new Vector2(_cloudStats.DisappearLineInM - 0.01f, 0);
            Assert.IsTrue(_teleporter.ShouldTeleportCloud());
        }

        [Test]
        public void TeleportCloud()
        {
            _cloud.Position = new Vector2(_cloudStats.DisappearLineInM - 0.01f, 7);

            _teleporter.TeleportCloud();

            Assert.AreEqual(new Vector2(_cloudStats.ReappaerLineInM, 7), _cloud.Position);
        }
    }
}
