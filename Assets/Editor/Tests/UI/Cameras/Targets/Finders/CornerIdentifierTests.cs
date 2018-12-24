using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Finders;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Finders
{
    public class CornerIdentifierTests
    {
        private ICornerIdentifier _cornerIdentifier;
        private ICornerCutoffProvider _cornerCutoffProvider;
        private ICameraTarget _cameraTarget;

        [SetUp]
        public void TestSetup()
        {
            _cornerCutoffProvider = Substitute.For<ICornerCutoffProvider>();
            _cornerIdentifier = new CornerIdentifier(_cornerCutoffProvider);

            _cameraTarget = Substitute.For<ICameraTarget>();

            _cornerCutoffProvider.AICruiserCornerXPositionCutoff.Returns(12);
            _cornerCutoffProvider.PlayerCruiserCornerXPositionCutoff.Returns(-12);
            _cornerCutoffProvider.OverviewOrthographicSizeCutoff.Returns(77);
        }

        [Test]
        public void FindCorner_PlayerCruiserCorner()
        {
            float xCutoff = _cornerCutoffProvider.PlayerCruiserCornerXPositionCutoff;
            _cameraTarget.Position.Returns(new Vector3(xCutoff, 0, 0));
            CameraCorner? cameraCorner = _cornerIdentifier.FindCorner(_cameraTarget);
            Assert.AreEqual(CameraCorner.PlayerCruiser, cameraCorner);
        }

        [Test]
        public void FindCorner_AICruiserCorner()
        {
            float xCutoff = _cornerCutoffProvider.AICruiserCornerXPositionCutoff;
            _cameraTarget.Position.Returns(new Vector3(xCutoff, 0, 0));
            CameraCorner? cameraCorner = _cornerIdentifier.FindCorner(_cameraTarget);
            Assert.AreEqual(CameraCorner.AICruiser, cameraCorner);
        }

        [Test]
        public void FindCorner_OverviewCorner()
        {
            _cameraTarget.Position.Returns(new Vector3(0, 0, 0));
            float cutoff = _cornerCutoffProvider.OverviewOrthographicSizeCutoff;
            _cameraTarget.OrthographicSize.Returns(cutoff);
            CameraCorner? cameraCorner = _cornerIdentifier.FindCorner(_cameraTarget);
            Assert.AreEqual(CameraCorner.Overview, cameraCorner);
        }

        [Test]
        public void FindCorner_NoCorner()
        {
            _cameraTarget.Position.Returns(new Vector3(0, 0, 0));
            _cameraTarget.OrthographicSize.Returns(_cornerCutoffProvider.OverviewOrthographicSizeCutoff - 1);
            CameraCorner? cameraCorner = _cornerIdentifier.FindCorner(_cameraTarget);
            Assert.IsNull(cameraCorner);
        }
    }
}