using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Adjusters
{
    public class InstantCameraAdjusterTests
    {
        private ICameraAdjuster _adjuster;
        private ICameraTargetProvider _cameraTargetProvider;
        private ICamera _camera;
        private ICameraTarget _cameraTarget;
        private int _adjustmentCompletedCounter;

        [SetUp]
        public void TestSetup()
        {
            _cameraTargetProvider = Substitute.For<ICameraTargetProvider>();
            _camera = Substitute.For<ICamera>();
            _adjuster = new InstantCameraAdjuster(_cameraTargetProvider, _camera);

            _cameraTarget = new CameraTarget(position: new Vector3(1, 2, 3), orthographicSize: 4);
            _cameraTargetProvider.Target.Returns(_cameraTarget);

            _adjustmentCompletedCounter = 0;
            _adjuster.CompletedAdjustment += (sender, e) => _adjustmentCompletedCounter++;
        }

        [Test]
        public void AdjustCamera()
        {
            _adjuster.AdjustCamera();

            Assert.AreEqual(_cameraTarget.Position, _camera.Position);
            Assert.AreEqual(_cameraTarget.OrthographicSize, _camera.OrthographicSize);
            Assert.AreEqual(1, _adjustmentCompletedCounter);
        }
    }
}