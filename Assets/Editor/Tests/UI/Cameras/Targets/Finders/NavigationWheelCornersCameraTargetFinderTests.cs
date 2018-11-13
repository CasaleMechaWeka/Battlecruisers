using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Finders;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Finders
{
    public class NavigationWheelCornersCameraTargetFinderTests
    {
        private ICameraTargetFinder _cornerTargetFinder, _coreTargetFinder;
        private ICornerIdentifier _cornerIdentifier;
        private ICornerCameraTargetProvider _cornerCameraTargetProvider;
        private ICameraTarget _originalCameraTarget, _modifiedCameraTarget;

        [SetUp]
        public void TestSetup()
        {
            _coreTargetFinder = Substitute.For<ICameraTargetFinder>();
            _cornerIdentifier = Substitute.For<ICornerIdentifier>();
            _cornerCameraTargetProvider = Substitute.For<ICornerCameraTargetProvider>();

            _cornerTargetFinder = new NavigationWheelCornersCameraTargetFinder(_coreTargetFinder, _cornerIdentifier, _cornerCameraTargetProvider);

            _originalCameraTarget = Substitute.For<ICameraTarget>();
            _coreTargetFinder.FindCameraTarget().Returns(_originalCameraTarget);

            _modifiedCameraTarget = Substitute.For<ICameraTarget>();
        }

        [Test]
        public void FindCameraTarget_NotInCorner_ReturnsUnmodifiedTarget()
        {
            _cornerIdentifier.FindCorner(_originalCameraTarget).Returns((CameraCorner?)null);

            ICameraTarget actualTarget = _cornerTargetFinder.FindCameraTarget();

            Assert.AreSame(_originalCameraTarget, actualTarget);
        }

        [Test]
        public void FindCameraTarget_InCorner_ReturnsModifiedTarget()
        {
            CameraCorner cameraCorner = CameraCorner.Overview;
            _cornerIdentifier.FindCorner(_originalCameraTarget).Returns(cameraCorner);
            _cornerCameraTargetProvider.GetTarget(cameraCorner).Returns(_modifiedCameraTarget);

            ICameraTarget actualTarget = _cornerTargetFinder.FindCameraTarget();

            Assert.AreSame(_modifiedCameraTarget, actualTarget);
        }
    }
}