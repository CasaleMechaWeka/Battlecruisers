using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
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
        private int _adjustmentCompletedCounter;

        [SetUp]
        public void TestSetup()
        {
            _cameraTargetProvider = Substitute.For<ICameraTargetProvider>();
            _zoomAdjuster = Substitute.For<ISmoothZoomAdjuster>();
            _positionAdjuster = Substitute.For<ISmoothPositionAdjuster>();
            _adjuster = new SmoothCameraAdjuster(_cameraTargetProvider, _zoomAdjuster, _positionAdjuster);

            _cameraTarget = new CameraTarget(position: new Vector3(1, 2, 3), orthographicSize: 4);
            _cameraTargetProvider.Target.Returns(_cameraTarget);

            _adjustmentCompletedCounter = 0;
            _adjuster.CompletedAdjustment += (sender, e) => _adjustmentCompletedCounter++;
        }

        [Test]
        public void AdjustCamera_ReachedTarget_EmitsEvent()
        {
            _zoomAdjuster.AdjustZoom(_cameraTarget.OrthographicSize).Returns(true);
            _positionAdjuster.AdjustPosition(_cameraTarget.Position).Returns(true);

            _adjuster.AdjustCamera();

            _zoomAdjuster.Received().AdjustZoom(_cameraTarget.OrthographicSize);
            _positionAdjuster.Received().AdjustPosition(_cameraTarget.Position);

            Assert.AreEqual(1, _adjustmentCompletedCounter);
        }

        [Test]
        public void AdjustCamera_HaveNotReachedTarget_DoesNotEmitEvent()
        {
            _zoomAdjuster.AdjustZoom(_cameraTarget.OrthographicSize).Returns(true);
            _positionAdjuster.AdjustPosition(_cameraTarget.Position).Returns(false);

            _adjuster.AdjustCamera();

            _zoomAdjuster.Received().AdjustZoom(_cameraTarget.OrthographicSize);
            _positionAdjuster.Received().AdjustPosition(_cameraTarget.Position);

            Assert.AreEqual(0, _adjustmentCompletedCounter);
        }
    }
}