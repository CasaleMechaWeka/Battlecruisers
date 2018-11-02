using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Cameras.Adjusters;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Adjusters
{
    public class SmoothCameraAdjusterTests
    {
        private ICameraAdjuster _adjuster;
        private ICameraTargetProvider _cameraTargetProvider;
        private ICameraTarget _cameraTarget;
        private ISmoothZoomAdjuster _zoomAdjuster;
        private ISmoothPositionAdjuster _positionAdjuster;

        [SetUp]
        public void TestSetup()
        {
            _cameraTargetProvider = Substitute.For<ICameraTargetProvider>();
            _zoomAdjuster = Substitute.For<ISmoothZoomAdjuster>();
            _positionAdjuster = Substitute.For<ISmoothPositionAdjuster>();
            _adjuster = new SmoothCameraAdjuster(_cameraTargetProvider, _zoomAdjuster, _positionAdjuster);

            _cameraTarget = new CameraTarget(position: new Vector3(1, 2, 3), orthographicSize: 4);
            _cameraTargetProvider.Target.Returns(_cameraTarget);
        }

        [Test]
        public void AdjustCamera()
        {
            _adjuster.AdjustCamera();

            _zoomAdjuster.Received().AdjustZoom(_cameraTarget.OrthographicSize);
            _positionAdjuster.Received().AdjustPosition(_cameraTarget.Position);
        }
    }
}