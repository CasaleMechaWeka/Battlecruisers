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
        private ICameraTarget _cameraTarget;

// FELIX  Fix :D
        //[SetUp]
        //public void TestSetup()
        //{
        //    _cornerIdentifier = new CornerIdentifier();
        //    _cameraTarget = Substitute.For<ICameraTarget>();
        //}

        //[Test]
        //public void FindCorner_PlayerCruiserCorner()
        //{
        //    _cameraTarget.Position.Returns(new Vector3(CornerIdentifier.PLAYER_CRUISER_CORNER_X_POSITION_CUTOFF, 0, 0));
        //    CameraCorner? cameraCorner = _cornerIdentifier.FindCorner(_cameraTarget);
        //    Assert.AreEqual(CameraCorner.PlayerCruiser, cameraCorner);
        //}

        //[Test]
        //public void FindCorner_AICruiserCorner()
        //{
        //    _cameraTarget.Position.Returns(new Vector3(CornerIdentifier.AI_CRUISER_CORNER_X_POSITION_CUTOFF, 0, 0));
        //    CameraCorner? cameraCorner = _cornerIdentifier.FindCorner(_cameraTarget);
        //    Assert.AreEqual(CameraCorner.AICruiser, cameraCorner);
        //}

        //[Test]
        //public void FindCorner_OverviewCorner()
        //{
        //    _cameraTarget.Position.Returns(new Vector3(0, 0, 0));
        //    _cameraTarget.OrthographicSize.Returns(CornerIdentifier.OVERVIEW_ORTHOGRAPHIC_SIZE_CUTOFF);
        //    CameraCorner? cameraCorner = _cornerIdentifier.FindCorner(_cameraTarget);
        //    Assert.AreEqual(CameraCorner.Overview, cameraCorner);
        //}

        //[Test]
        //public void FindCorner_NoCorner()
        //{
        //    _cameraTarget.Position.Returns(new Vector3(0, 0, 0));
        //    _cameraTarget.OrthographicSize.Returns(CornerIdentifier.OVERVIEW_ORTHOGRAPHIC_SIZE_CUTOFF - 1);
        //    CameraCorner? cameraCorner = _cornerIdentifier.FindCorner(_cameraTarget);
        //    Assert.IsNull(cameraCorner);
        //}
    }
}