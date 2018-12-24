using BattleCruisers.Cruisers;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Finders
{
    public class CornerCameraTargetProviderTests
    {
        private ICornerCameraTargetProvider _targetProvider;
        private ICameraTarget _expectedOverviewTarget, _expectedPlayerCruiserTarget, _expectedAICruiserTarget;

        [SetUp]
        public void TestSetup()
        {
            ICamera camera = Substitute.For<ICamera>();
            ICameraCalculator cameraCalculator = Substitute.For<ICameraCalculator>();
            ICruiser playerCruiser = Substitute.For<ICruiser>();
            ICruiser aiCruiser = Substitute.For<ICruiser>();

            // Overview
            Vector3 cameraPosition = new Vector3(1, 2, 3);
            camera.Transform.Position = cameraPosition;
            camera.OrthographicSize = 33;
            cameraCalculator.FindCameraYPosition(camera.OrthographicSize).Returns(7);
            Vector3 expectedOverviewPosition = new Vector3(1, 7, 3);
            _expectedOverviewTarget = new CameraTarget(expectedOverviewPosition, camera.OrthographicSize);

            // Player cruiser
            _expectedPlayerCruiserTarget = FindExpectedCruiserTarget(camera, cameraCalculator, playerCruiser);

            // AI cruiser
            _expectedAICruiserTarget = FindExpectedCruiserTarget(camera, cameraCalculator, aiCruiser);

            // FELIX  Update tests :)
            _targetProvider = new CornerCameraTargetProvider(camera, cameraCalculator, null, playerCruiser, aiCruiser);
        }

        private ICameraTarget FindExpectedCruiserTarget(ICamera camera, ICameraCalculator cameraCalculator, ICruiser cruiser)
        {
            float expectedOrthographicSize = cruiser.GetHashCode();
            cameraCalculator.FindCameraOrthographicSize(cruiser).Returns(expectedOrthographicSize);
            Vector3 expectedPosition = new Vector3(9, 8, cruiser.GetHashCode());
            cameraCalculator
                .FindCruiserCameraPosition(cruiser, expectedOrthographicSize, camera.Transform.Position.z)
                .Returns(expectedPosition);
            return new CameraTarget(expectedPosition, expectedOrthographicSize);
        }

        [Test]
        public void GetTarget_Overview()
        {
            Assert.AreEqual(_expectedOverviewTarget, _targetProvider.GetTarget(CameraCorner.Overview));
        }

        [Test]
        public void GetTarget_PlayerCruiser()
        {
            Assert.AreEqual(_expectedPlayerCruiserTarget, _targetProvider.GetTarget(CameraCorner.PlayerCruiser));
        }

        [Test]
        public void GetTarget_AICruiser()
        {
            Assert.AreEqual(_expectedAICruiserTarget, _targetProvider.GetTarget(CameraCorner.AICruiser));
        }
    }
}